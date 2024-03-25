invs = env['account.move'].search([
    ('name', '!=', False),
    ('name', 'like', '______________________________%')
], limit=10, order='id asc')