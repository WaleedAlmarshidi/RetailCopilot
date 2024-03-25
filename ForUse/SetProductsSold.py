import logging 
import requests 
from odoo import api, models, http 
import json 
  
_logger = logging.getLogger(__name__) 
  
def send_request_in_background(url, payload): 
    try: 
        response = requests.patch( 
            url, 
            json=payload, 
            headers={"content-type": "application/json"} 
        ) 
        _logger.info(f"Request successfully sent to BusinessVar or ZUSE: {payload}, Status: {response.status_code}, Msg: {response.text}") 
    except Exception as e: 
        _logger.exception(f"Could not send request: {e}") 
  
def correct_phone(phone_number): 
    arabic_to_western_digits = { 
        '٠': '0', 
        '١': '1', 
        '٢': '2', 
        '٣': '3', 
        '٤': '4', 
        '٥': '5', 
        '٦': '6', 
        '٧': '7', 
        '٨': '8', 
        '٩': '9' 
    } 
    phone_number = phone_number.replace(' ', '')
    for arabic, western in arabic_to_western_digits.items(): 
        phone_number = phone_number.replace(arabic, western) 
    return phone_number 
  
def prepare_webhook_payload(pos_order): 
    global products_details
    if "line_note" in pos_order.lines:
        products_details = pos_order.lines.read([
            "price_unit", "qty", "price_subtotal", "discount",
            "product_id"
        ])
    else:
        products_details = pos_order.lines.read([
            "price_unit", "qty", "price_subtotal", "discount",
            "product_id"
        ])
        
    for rec in products_details:
        product_details = rec.pop("product_id")
        product_id = self.env["product.product"].browse(product_details[0])

        rec.update({
            "category": product_id.pos_categ_id.name or "",
            "full_product_name": product_details[1]
        })

    return { 
        "ID": pos_order.id, 
        "Date": (pos_order.date_order or fields.Datetime.now()).strftime('%Y-%m-%d %H:%M:%S'), 
        "CompanyName": pos_order.company_id.name, 
        "PosId": pos_order.config_id.uuid, 
        "CustomerName": pos_order.partner_id.name or None, 
        "CustomerID": pos_order.partner_id.id if pos_order.partner_id else -1, 
        "Margin": pos_order.margin or None, 
        "Total": pos_order.amount_total, 
        "CustomerPhone": correct_phone(pos_order.partner_id.phone) if pos_order.partner_id.phone else None, 
        "PurchasedProductsSerialised": products_details, 
        "CustomerPosOrderCount": pos_order.partner_id.pos_order_count, 
    } 

pos_orders = env['pos.order'].search([("bv_is_synced", '=', False), ('partner_id', '!=', False), ('partner_id.phone', '!=', False)], order='date_order asc')
for pos_order in pos_orders:
    payload = prepare_webhook_payload(pos_order)
    url = "https://retailcopilot.azurewebsites.net/api/pos" 
    send_request_in_background(url, payload)    
    pos_order.write({"bv_is_synced": True})
    env.cr.commit()
