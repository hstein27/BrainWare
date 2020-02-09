CREATE PROCEDURE [dbo].[GetOrders]
	@companyId int
AS
	SELECT c.name, o.description, o.order_id, op.price,
                            op.product_id, op.quantity, p.name, p.price 
                            FROM Company c INNER JOIN[Order] o on c.company_id = o.company_id
                            inner join [orderproduct] op on o.order_id = op.order_id
                            INNER JOIN [Product] p on op.product_id = p.product_id where o.company_id = @companyId
RETURN
