-- Thêm dữ liệu mẫu cho Users
INSERT INTO Users (FullName, Email, Phone, Password, Address, Birthday, Gender, Role, IsActive)
VALUES 
    (N'Admin', 'nhom11@gmail.com', '0123456789', '12345678', N'Hà Nội', '1990-01-01', 'Nam', 'Admin', 1),
    (N'Thái Văn Trường', 'truongg9655@gmail.com', '0987654321', '12345678', N'Hồ Chí Minh', '1995-05-15', 'Nam', 'User', 1),
    (N'Trần Thị Dịu', 'tranthib@gmail.com', '0912345678', '12345678', N'Đà Nẵng', '1992-08-20', N'Nữ', 'User', 1),
    (N'Lê Văn Cức', 'levanc@gmail.com', '0898765432', '12345678', N'Hải Phòng', '1988-12-10', 'Nam', 'User', 1),
    (N'Phạm Thị Dàng', 'phamthid@gmail.com', '0865432109', '12345678', N'Cần Thơ', '1993-03-25', N'Nữ', 'User', 1);


-- Thêm dữ liệu mẫu cho Categories
INSERT INTO Categories (CategoryName, Description, IsActive)
VALUES 
    (N'iPhone', N'Điện thoại iPhone chính hãng', 1),
    (N'Samsung', N'Điện thoại Samsung chính hãng', 1),
    (N'Xiaomi', N'Điện thoại Xiaomi chính hãng', 1),
    (N'OPPO', N'Điện thoại OPPO chính hãng', 1),
    (N'Vivo', N'Điện thoại Vivo chính hãng', 1);

-- Thêm dữ liệu mẫu cho Products
INSERT INTO Products (CategoryID, ProductName, Description, OriginalPrice, SalePrice, DefaultImageUrl, Stock, IsActive)
VALUES 
    -- iPhone
    (1, N'iPhone 14 Pro Max', 
     N'iPhone 14 Pro Max - Flagship mới nhất của Apple với camera 48MP, chip A16 Bionic', 
     17900000, 15990000, 
     '/assets/images/iphone-14-promax-den.png', 
     50, 1),
    
    (1, N'iPhone 16 Pro Max', 
     N'iPhone 16 Pro Max - Dynamic Island, camera 48MP, màn hình Always On', 
     30990000, 27990000, 
     '/assets/images/iphone-16-promax-samac.png', 
     45, 1),
    
    (1, N'iPhone 16 plus', 
     N'iPhone 16 - Hiệu năng mạnh mẽ, camera kép 12MP', 
     25990000, 23990000, 
     '/assets/images/iphone-16-plus-hong.png', 
     40, 1),
    
    -- Samsung
    (2, N'Samsung Galaxy S24 Ultra', 
     N'Samsung Galaxy S24 Ultra - Camera 200MP, bút S-Pen', 
     15990000, 14990000, 
     '/assets/images/samsung-s24-ultra-tim.png', 
     35, 1),
    
    (2, N'Samsung Galaxy S24 Plus', 
     N'Samsung Galaxy S24 Plus - Snapdragon 8 Gen 2, pin 4700mAh', 
     19990000, 18990000, 
     '/assets/images/samsung-s24-plus-tim.png', 
     30, 1),
    
    (2, N'Samsung Galaxy Z Fold6', 
     N'Samsung Galaxy Z Fold6 - Màn hình gập cao cấp', 
     10990000, 7990000, 
     '/assets/images/samsung-zfold6-xanhnavy.png', 
     25, 1),
    
    -- Xiaomi
    (3, N'Xiaomi Redmi note 13 Pro', 
     N'Xiaomi 13 Redmi note 13 Pro - Camera Leica 50MP, sạc 120W', 
     10990000, 9990000, 
     '/assets/images/xiaomi-redminote13pro-trang.png', 
     40, 1),
    
    (3, N'Xiaomi 14T Pro', 
     N'Xiaomi 14T Pro - Camera 200MP, Snapdragon 8+ Gen 1', 
     8990000, 7990000, 
     '/assets/images/xiaomi-14tpro-xam.png', 
     35, 1),
    
    (3, N'Redmi Poco', 
     N'Redmi Poco - Camera 108MP, màn hình AMOLED 120Hz', 
     7990000, 5990000, 
     '/assets/images/xiaomi-poco-xanh.png', 
     60, 1),
    
    -- OPPO
    
    (4, N'OPPO Reno12 Pro 5G ', 
     N'OPPO Reno12 Pro - Camera 50MP, sạc 80W', 
     17990000, 15990000,
     '/assets/images/oppo-reno12-bac.png', 
     40, 1),
    
    (4, N'OPPO Reno11 5G', 
     N'OPPO Reno11 5G - Pin 5000mAh, RAM 8GB', 
     6990000, 5990000, 
     '/assets/images/oppo-reno11-5g-xanhden.png', 
     50, 1),
    
	(4, N'OPPO A78', 
     N'OPPO A90 - Pin 5000mAh, RAM 8GB', 
     6990000, 5990000, 
     '/assets/images/oppo-A78-den.png', 
     50, 1),
    -- Vivo
    (5, N'Vivo Y19S', 
     N'Vivo Y19S - Camera Zeiss, chip MediaTek 9200', 
     19990000, 18990000, 
     '/assets/images/vivo-y19s-sky.png', 
     35, 1),
    
    (5, N'Vivo V29e Pro', 
     N'Vivo V29e Pro - Camera 64MP, màn hình cong', 
     13990000, 12990000, 
     '/assets/images/vivo-v29e-xanhduong.png', 
     45, 1),
    
    (5, N'Vivo Y03', 
     N'Vivo Y03 - Pin 5000mAh, sạc 44W', 
     6490000, 5990000, 
     '/assets/images/vivo-y03-den.png', 
     55, 1);


INSERT INTO ProductVariants (ProductID, Color, ColorCode, Storage, Price, Stock, Sold, VarImageUrl)
VALUES 
    -- iPhone 14 Pro Max variants
    (1, N'Đen', '#000000', '6GB Ram - 256', 17990000, 20, 2,'~/assets/images/iphone-14-promax-den.png'),
	(1, N'Đen', '#000000', '6GB Ram - 128', 16990000, 20, 2,'~/assets/images/iphone-14-promax-den.png'),
    (1, N'Trắng', '#FFFFFF', '6GB Ram - 128', 15990000, 15, 5, '~/assets/images/iphone-14-promax-trang.png'),
	(1, N'Trắng', '#FFFFFF', '6GB Ram - 256', 16990000, 15, 5, '~/assets/images/iphone-14-promax-trang.png'),
    (1, N'Gold', '#F9E79F', '6GB Ram - 256', 17990000, 10, 4,'~/assets/images/iphone-14-promax-vang.png'),
	(1, N'Gold', '#F9E79F', '6GB Ram - 128', 16990000, 10, 4,'~/assets/images/iphone-14-promax-vang.png'),
    
    -- iPhone 16 Pro Max variants
	(2, N'Đen', '#000000', '8GB Ram - 256', 28990000, 20, 2,'~/assets/images/iphone-16-promax-den.png'),
	(2, N'Đen', '#000000', '8GB Ram - 128', 27990000, 20, 2,'~/assets/images/iphone-16-promax-den.png'),
    (2, N'Trắng', '#FFFFFF', '8GB Ram - 128', 27990000, 10, 5, '~/assets/images/iphone-16-promax-trang.png'),
	(2, N'Trắng', '#FFFFFF', '8GB Ram - 256', 28990000, 15, 5, '~/assets/images/iphone-16-promax-trang.png'),
    (2, N'Sa mạc', '#EDC9AF', '8GB Ram - 256', 30990000, 12, 4,'~/assets/images/iphone-16-promax-samac.png'),
	(2, N'Sa mạc', '#EDC9AF', '8GB Ram - 128', 28990000, 10, 4,'~/assets/images/iphone-16-promax-samac.png'),
   
    -- iPhone 16 Plus variants
    (3, N'Đen', '#000000', '8GB Ram - 256', 24990000, 20, 2,'~/assets/images/iphone-16-plus-den.png'),
	(3, N'Đen', '#000000', '8GB Ram - 128', 23990000, 20, 2,'~/assets/images/iphone-16-plus-den.png'),
    (3, N'Hồng', '#FFC0CB', '8GB Ram - 128', 23990000, 15, 4, '~/assets/images/iphone-16-plus-trang.png'),
	(3, N'Hồng', '#FFC0CB', '8GB Ram - 256', 24990000, 17, 5, '~/assets/images/iphone-16-plus-trang.png'),
    (3, N'Xanh lưu ly', '#4169E1', '8GB Ram - 256', 24990000, 10, 0,'~/assets/images/iphone-16-plus-xanhluuly.png'),
	(3, N'Xanh lưu ly', '#4169E1', '8GB Ram - 128', 25990000, 10, 1,'~/assets/images/iphone-16-plus-xanhluuly.png'),
    

	-- Thêm tiếp cho đủ 15ID
 -- Samsung Galaxy S24 Ultra variants
    (4, N'Xám', '#A9A9A9', '12GB Ram - 256', 15990000, 20, 2,'~/assets/images/samsung-s24-ultra-xam.png'),
    (4, N'Xám', '#A9A9A9', '12GB Ram - 512', 16990000, 15, 1,'~/assets/images/samsung-s24-ultra-xam.png'),
    (4, N'Tím', '#9370DB', '12GB Ram - 256', 15990000, 15, 3,'~/assets/images/samsung-s24-ultra-tim.png'),
    (4, N'Tím', '#9370DB', '12GB Ram - 512', 16990000, 10, 2,'~/assets/images/samsung-s24-ultra-tim.png'),
    (4, N'Vàng', '#FFC107', '12GB Ram - 256', 15990000, 12, 4,'~/assets/images/samsung-s24-ultra-vang.png'),
    (4, N'Vàng', '#FFC107', '12GB Ram - 512', 16990000, 8, 3,'~/assets/images/samsung-s24-ultra-vang.png'),

    -- Samsung Galaxy S24 Plus variants
    (5, N'Xám', '#808080', '8GB Ram - 256', 19990000, 15, 2,'~/assets/images/samsung-s24-plus-xam.png'),
    (5, N'Xám', '#808080', '8GB Ram - 512', 20990000, 10, 1,'~/assets/images/samsung-s24-plus-xam.png'),
    (5, N'Tím', '#800080', '8GB Ram - 256', 19990000, 12, 3,'~/assets/images/samsung-s24-plus-tim.png'),
    (5, N'Tím', '#800080', '8GB Ram - 512', 20990000, 8, 2,'~/assets/images/samsung-s24-plus-tim.png'),

    -- Xiaomi Redmi Note 13 Pro variants
    (7, N'Tím', '#DDA0DD', '8GB Ram - 256', 10990000, 20, 5,'~/assets/images/xiaomi-redminote13pro-tim.png'),
    (7, N'Tím', '#DDA0DD', '12GB Ram - 256', 11990000, 15, 3,'~/assets/images/xiaomi-redminote13pro-tim.png'),
    (7, N'Trắng', '#FFFFFF', '8GB Ram - 256', 10990000, 18, 4,'~/assets/images/xiaomi-redminote13pro-trang.png'),
    (7, N'Trắng', '#FFFFFF', '12GB Ram - 256', 11990000, 12, 2,'~/assets/images/xiaomi-redminote13pro-trang.png'),

    -- OPPO Reno12 Pro variants
    (10, N'Bạc', '#C0C0C0', '12GB Ram - 256', 17990000, 15, 3,'~/assets/images/oppo-reno12-bac.png'),
    (10, N'Bạc', '#C0C0C0', '12GB Ram - 512', 18990000, 10, 2,'~/assets/images/oppo-reno12-bac.png'),
    (10, N'Nâu', '#654321', '12GB Ram - 256', 17990000, 18, 4,'~/assets/images/oppo-reno12-nau.png'),
    (10, N'Nâu', '#654321', '12GB Ram - 512', 18990000, 12, 3,'~/assets/images/oppo-reno12-nau.png'),

    -- Vivo Y19S variants
    (13, N'Xanh', '#0000FF', '8GB Ram - 128', 19990000, 20, 4,'~/assets/images/vivo-y19s-sky.png'),
    (13, N'Xanh', '#0000FF', '8GB Ram - 256', 20990000, 15, 2,'~/assets/images/vivo-y19s-sky.png'),
    (13, N'Trắng', '#FFFFFF', '8GB Ram - 128', 19990000, 18, 3,'~/assets/images/vivo-y19s-rainbow.png'),
    (13, N'Trắng', '#FFFFFF', '8GB Ram - 256', 20990000, 12, 1,'~/assets/images/vivo-y19s-rainbow.png'),
    -- Samsung Galaxy Z Fold6 variants
    (6, N'Hồng', '#FFB6C1', '12GB Ram - 256', 10990000, 15, 2,'~/assets/images/samsung-zfold6-hongrose.png'),
    (6, N'Hồng', '#FFB6C1', '12GB Ram - 512', 11990000, 10, 1,'~/assets/images/samsung-zfold6-hongrose.png'),
    (6, N'Xanh', '#2C3E50', '12GB Ram - 256', 10990000, 12, 3,'~/assets/images/samsung-zfold6-xanhnavy.png'),
	(6, N'Xanh', '#2C3E50', '12GB Ram - 512', 11990000, 8, 2,'~/assets/images/samsung-zfold6-xanhnavy.png'),

    -- Xiaomi 14T Pro variants
    (8, N'Xám', '#808080', '12GB Ram - 256', 8990000, 18, 4,'~/assets/images/xiaomi-14tpro-xam.png'),
    (8, N'Xám', '#808080', '12GB Ram - 512', 9990000, 12, 2,'~/assets/images/xiaomi-14tpro-xam.png'),

    -- Redmi Poco variants
    (9, N'Xanh', '#87CEEB', '8GB Ram - 128', 7990000, 25, 6,'~/assets/images/xiaomi-poco-xanh.png'),
    (9, N'Xanh', '#87CEEB', '8GB Ram - 256', 8990000, 20, 4,'~/assets/images/xiaomi-poco-xanh.png'),
    (9, N'Đen', '#000000', '8GB Ram - 128', 7990000, 22, 5,'~/assets/images/xiaomi-poco-den.png'),
    (9, N'Đen', '#000000', '8GB Ram - 256', 8990000, 18, 3,'~/assets/images/xiaomi-poco-den.png'),

    -- OPPO Reno11 5G variants
    (11, N'Lục đen', '#006400', '8GB Ram - 256', 6990000, 20, 4,'~/assets/images/oppo-reno11-5g-xanhden.png'),
    (11, N'Lục đen', '#006400', '12GB Ram - 256', 7990000, 15, 3,'~/assets/images/oppo-reno11-5g-xanhden.png'),
    (11, N'Xanh', '#87CEEB', '8GB Ram - 256', 6990000, 18, 5,'~/assets/images/oppo-reno11-5g-xanh.png'),
    (11, N'Xanh', '#87CEEB', '12GB Ram - 256', 7990000, 12, 2,'~/assets/images/oppo-reno11-5g-xanh.png'),

    -- OPPO A78 variants
    (12, N'Đen', '#000000', '8GB Ram - 128', 6990000, 25, 8,'~/assets/images/oppo-a78-den.png'),
    (12, N'Đen', '#000000', '8GB Ram - 256', 7990000, 20, 5,'~/assets/images/oppo-a78-den.png'),

    -- Vivo V29e Pro variants
    (14, N'Xanh dương', '#0000FF', '8GB Ram - 256', 13990000, 20, 4,'~/assets/images/vivo-v29e-xanhduong.png'),
    (14, N'Xanh dương', '#0000FF', '12GB Ram - 256', 14990000, 15, 3,'~/assets/images/vivo-v29e-xanhduong.png'),
    (14, N'Đen', '#000000', '8GB Ram - 256', 13990000, 18, 5,'~/assets/images/vivo-v29e-den.png'),
    (14, N'Đen', '#000000', '12GB Ram - 256', 14990000, 12, 2,'~/assets/images/vivo-v29e-den.png'),

    -- Vivo Y03 variants
    (15, N'Đen', '#000000', '4GB Ram - 64', 6490000, 25, 7,'~/assets/images/vivo-y03-den.png'),
    (15, N'Đen', '#000000', '4GB Ram - 128', 6990000, 20, 5,'~/assets/images/vivo-y03-den.png'),
    (15, N'Xanh', '#ADD8E6', '4GB Ram - 64', 6490000, 22, 6,'~/assets/images/vivo-y03-xanhla.png'),
    (15, N'Xanh', '#ADD8E6', '4GB Ram - 128', 6990000, 18, 4,'~/assets/images/vivo-y03-xanhla.png');

	-- Thêm dữ liệu mẫu cho ProductSpecs
	INSERT INTO ProductSpecs (ProductID, SpecName, SpecValue)
	VALUES
	-- iPhone 14 Pro Max specs
	(1, N'Màn hình', N'OLED 6.7 inch Super Retina XDR'),
	(1, N'CPU', N'Apple A16 Bionic'),
	(1, N'RAM', N'6GB'),
	(1, N'Camera sau', N'Chính 48 MP & Phụ 12 MP, 12 MP'),
	(1, N'Camera trước', N'12 MP'),
	(1, N'Pin', N'4323 mAh'),
	-- Samsung S23 Ultra specs
	(4, N'Màn hình', N'Dynamic AMOLED 2X 6.8 inch'),
	(4, N'CPU', N'Snapdragon 8 Gen 2'),
	(4, N'RAM', N'8GB/12GB'),
	(4, N'Camera sau', N'Chính 200 MP & Phụ 12 MP, 10 MP, 10 MP'),
	(4, N'Camera trước', N'12 MP'),
	(4, N'Pin', N'5000 mAh'),
	-- Xiaomi 13 Pro specs
	(7, N'Màn hình', N'AMOLED 6.73 inch 2K'),
	(7, N'CPU', N'Snapdragon 8 Gen 2'),
	(7, N'RAM', N'12GB'),
	(7, N'Camera sau', N'Chính 50 MP & Phụ 50 MP, 50 MP'),
	(7, N'Camera trước', N'32 MP'),
	(7, N'Pin', N'4820 mAh');
-- Thêm dữ liệu mẫu cho Orders
INSERT INTO Orders (UserID, Status, ShippingAddress, ShippingPhone, TotalAmount, ShippingFee)
VALUES
(2, N'Đã giao', N'123 Đường ABC, Quận 1, TP.HCM', '0987654321', 27990000, 30000),
(3, N'Chờ xác nhận', N'456 Đường XYZ, Quận Hải Châu, Đà Nẵng', '0912345678', 17990000, 30000),
(4, N'Đang giao', N'789 Đường DEF, Quận Hồng Bàng, Hải Phòng', '0898765432', 24990000, 30000),
(2, N'Đã hủy', N'123 Đường ABC, Quận 1, TP.HCM', '0987654321', 20990000, 30000),
(3, N'Đã giao', N'456 Đường XYZ, Quận Hải Châu, Đà Nẵng', '0912345678', 35990000, 30000);
-- Thêm dữ liệu mẫu cho OrderDetails
INSERT INTO OrderDetails (OrderID, ProductID, VariantID, Quantity, Price)
VALUES
(1, 1, 1, 1, 27990000),
(2, 2, 4, 1, 17990000),
(3, 4, 6, 1, 24990000),
(4, 7, 9, 1, 20990000),
(5, 4, 8, 1, 35990000);
-- Thêm dữ liệu mẫu cho ProductImages
INSERT INTO ProductImages (ProductID, ImageUrl, IsMainImage)
VALUES
-- iPhone 14 Pro Max images
(1, '/assets/images/products/iphone-14-pro-max-1.jpg', 1),
(1, '/assets/images/products/iphone-14-pro-max-2.jpg', 0),
(1, '/assets/images/products/iphone-14-pro-max-3.jpg', 0),
-- Samsung S23 Ultra images
(4, '/assets/images/products/samsung-s23-ultra-1.jpg', 1),
(4, '/assets/images/products/samsung-s23-ultra-2.jpg', 0),
(4, '/assets/images/products/samsung-s23-ultra-3.jpg', 0),
-- Xiaomi 13 Pro images
(7, '/assets/images/products/xiaomi-13-pro-1.jpg', 1),
(7, '/assets/images/products/xiaomi-13-pro-2.jpg', 0),
(7, '/assets/images/products/xiaomi-13-pro-3.jpg', 0);
-- Thêm dữ liệu cho SystemStatistics
INSERT INTO SystemStatistics DEFAULT VALUES;

select * from Categories