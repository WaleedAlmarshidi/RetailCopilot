invs = env['account.move'].search([
    ('name', '!=', False),
    ('name', 'like', '______________________________%')
], limit=10, order='id asc')


count = 0
while True:
    domain = [
        ('edi_state', '=', 'to_send'),
        ('state', '=', 'posted'),
        ('invoice_type', 'in', ['restaurant', 'reservation', 'customer']),
        ('invoice_date', '>', '2024-03-01 00:00:00'),
    ]
    invs = env["account.move"].search(
        domain, limit=500, order='name asc'
    )
    if invs:
        for rec in invs:
            if rec.edi_error_message and "while chain head" in rec.edi_error_message:
                rec.action_retry_edi_documents_error()
                
            elif rec.edi_web_services_to_process:
                rec.button_process_edi_web_services()

            while rec.edi_error_message and "Read timed out." in rec.edi_error_message:
                rec.button_process_edi_web_services()
            env.cr.commit() 
            print(rec.name)
        # invs._cron_process_edi_to_send_recs()
        count += 500
        print(count)
    else:
        break

domain = [
        ('edi_state', '=', 'to_send'),
        ('state', '=', 'posted'),
        ('invoice_type', '=', 'restaurant'),
        ('create_date', '>', '2024-04-01 00:00:00')
    ]
invs = env["account.move"].search(
    domain
)
len(invs)

invs = env['account.move'].search([
   ('state', '=', 'draft'),
   ('x_studio_isvalidated', '=', True),
   ('invoice_type', '=', 'restaurant'),
   ('invoice_date', '>', '2024-06-01 00:00:00')
], order='name asc')
print(len(invs))

count = 0
while True:
    domain = [
        ('commercial_partner_id', '=', False)
    ]
    partners = env["res.partner"].search(
        domain, order='id asc', limit=5
    )
    if partners:
        partners._compute_commercial_partner()
        env.cr.commit()
        count += 5
        print(count)
    else:
        break

count = 0
while True:
    invs = env['account.move'].search([('move_type', '=', 'out_invoice'), ('l10n_sa_confirmation_datetime', '=', False), ('state', '=', 'posted')], order='id desc')

    if invs:
        for inv in invs:
            inv.l10n_sa_confirmation_datetime = inv.invoice_date
        env.cr.commit()
        invs.recompute()

        env.cr.commit()
        count += 20
        print(count)
    else:
        break

count = 0
limit = 5
dates = [
    # ('2024-04-01', '2024-04-02'),
    ('2024-04-04', '2024-04-04'),
    ('2024-04-08', '2024-04-08'),
    ('2024-04-10', '2024-04-12'),
    ('2024-04-14', '2024-04-17'),
    ('2024-04-19', '2024-04-19'),
    ('2024-04-21', '2024-04-22'),
    ('2024-04-26', '2024-04-28'),
]

for start_date, end_date in dates:
    while True:
        domain = [
            ('state', '=', 'draft'),
            ('invoice_type','=', 'customer'),
            ('invoice_date', '>=', f'{start_date} 00:00:00'),
            ('invoice_date', '<=', f'{end_date} 23:59:59')
        ]
        print(start_date, end_date)
        invs = env["account.move"].search(
            domain, order='name asc', limit=limit
        )
        print(f'length: {len(invs)}')
        
        if invs:
            invs.action_post()
            env.cr.commit()
            count += len(invs)  # Increment by the actual number of invoices processed
            print(count)
        else:
            break





# DO NOT USE IN PRODUCTION WITH NO APPROVAL FROM A SENIOR DEVELOPER, USE IN STAGE FIRST
# Get all installed modules
modules = env['ir.module.module'].search([('state', '=', 'installed')])

# Filter modules whose author contains 'Odoo'
modules_to_keep = modules.filtered(lambda m: 'Odoo' in (m.author or ''))

# Modules to uninstall are those not in 'modules_to_keep'
modules_to_uninstall = modules - modules_to_keep

# Uninstall each module not authored by 'Odoo'
for module in modules_to_uninstall:
    try:
        module.button_immediate_uninstall()
        print(f"Uninstalled {module.name}")
    except Exception as e:
        print(f"Failed to uninstall {module.name}: {str(e)}")

# Commit changes (be cautious with this in a production environment)
env.cr.commit()







OptionalProduct = env['optional.product']

# Get a specific record or multiple records
record = OptionalProduct.search([], limit=1)  # Adjust the search domain as necessary

# Manually trigger the computation of `entry_linked_id`
record.compute_entry_id()

# Alternatively, if you want to trigger all pending recomputation:
record.recompute()
env.cr.commit()