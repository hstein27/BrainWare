CREATE TABLE [dbo].[orderproduct]
(
	[order_id] int NOT NULL,
	[product_id] int NOT NULL,
	[price] decimal(18,2) NOT NULL,
	[quantity] int NOT NULL, 
    CONSTRAINT [PK_orderproduct] PRIMARY KEY ([order_id], [product_id]), 
    CONSTRAINT [FK_orderproduct_product] FOREIGN KEY ([product_id]) REFERENCES [Product]([product_id]), 
    CONSTRAINT [FK_orderproduct_order] FOREIGN KEY ([order_id]) REFERENCES [Order]([order_id])
)
