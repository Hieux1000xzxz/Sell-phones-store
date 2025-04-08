-- Tạo database
CREATE DATABASE BanDienThoaiDiDong
GO

USE BanDienThoaiDiDong
GO

-- Bảng Users (Người dùng)
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Phone VARCHAR(15) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200),
    Birthday DATE,
    Gender NVARCHAR(10),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
)

-- Bảng Categories (Danh mục sản phẩm)
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL,
    IconUrl NVARCHAR(200),
    Description NVARCHAR(500),
    IsActive BIT DEFAULT 1
)

-- Bảng Products (Sản phẩm)
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    ProductName NVARCHAR(200) NOT NULL,
    Description NTEXT,
    OriginalPrice DECIMAL(18,0) NOT NULL,
    SalePrice DECIMAL(18,0) NOT NULL,
    DefaultImageUrl NVARCHAR(500),
    Stock INT DEFAULT 0,
	Sold INT DEFAULT 0,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
)

-- Bảng ProductVariants (Biến thể sản phẩm - màu sắc, dung lượng)
CREATE TABLE ProductVariants (
    VariantID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    Color NVARCHAR(50),
    Storage NVARCHAR(50),
    Price DECIMAL(18,0),
    Stock INT DEFAULT 0,
	Sold INT DEFAULT 0,
	VarImageUrl NVARCHAR(500)
)

-- Bảng ProductSpecs (Thông số kỹ thuật)
CREATE TABLE ProductSpecs (
    SpecID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    SpecName NVARCHAR(100),
    SpecValue NVARCHAR(200)
)

-- Bảng Orders (Đơn hàng)
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    OrderDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT N'Chờ xác nhận',
    ShippingAddress NVARCHAR(200),
    ShippingPhone VARCHAR(15),
    TotalAmount DECIMAL(18,0),
    Note NVARCHAR(500)
)

-- Bảng OrderDetails (Chi tiết đơn hàng)
CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    VariantID INT FOREIGN KEY REFERENCES ProductVariants(VariantID),
    Quantity INT,
    Price DECIMAL(18,0)
)

-- Bảng ProductImages (Hình ảnh sản phẩm)
CREATE TABLE ProductImages (
    ImageID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    ImageUrl NVARCHAR(500),
    IsMainImage BIT DEFAULT 0
)
CREATE TABLE CartItems (
    CartItemID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    ProductID INT NOT NULL,
    VariantID INT,
    Quantity INT DEFAULT 1,
    DateAdded DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (VariantID) REFERENCES ProductVariants(VariantID)
)
-- Tạo bảng thống kê tổng hợp
CREATE TABLE SystemStatistics (
    StatID INT IDENTITY(1,1) PRIMARY KEY,
    TotalOrders INT DEFAULT 0,
    OrdersInProgress INT DEFAULT 0,
    OrdersCompleted INT DEFAULT 0,
    OrdersCancelled INT DEFAULT 0,
    TotalProducts INT DEFAULT 0,
    TotalCategories INT DEFAULT 0,
    TotalUsers INT DEFAULT 0,
    TotalActiveUsers INT DEFAULT 0,
    TotalInactiveUsers INT DEFAULT 0,
    TotalStock INT DEFAULT 0,
    TotalSold INT DEFAULT 0,
    TotalRevenue DECIMAL(18, 2) DEFAULT 0.00,
    LastUpdated DATETIME DEFAULT GETDATE()
)

-- Cập nhật thống kê tổng hợp
UPDATE SystemStatistics
SET 
    TotalOrders = (SELECT COUNT(*) FROM Orders),
    OrdersInProgress = (SELECT COUNT(*) FROM Orders WHERE Status = 'Chờ xác nhận'),
    OrdersCompleted = (SELECT COUNT(*) FROM Orders WHERE Status = 'Đã giao'),
    OrdersCancelled = (SELECT COUNT(*) FROM Orders WHERE Status = 'Đã hủy'),
    TotalProducts = (SELECT COUNT(*) FROM Products),
    TotalCategories = (SELECT COUNT(*) FROM Categories),
    TotalUsers = (SELECT COUNT(*) FROM Users),
    TotalStock = (SELECT SUM(Stock) FROM Products),
    TotalSold = (SELECT SUM(Sold) FROM Products),
    TotalRevenue = (SELECT SUM(od.Quantity * od.Price) 
                    FROM OrderDetails od
                    JOIN Orders o ON od.OrderID = o.OrderID 
                    WHERE o.Status = 'Đã giao'),
    LastUpdated = GETDATE()
WHERE StatID = 1;

GO
CREATE TRIGGER trg_UpdateProductStockSold
ON ProductVariants
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Tính tổng Stock và Sold khi thêm hoặc sửa
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        UPDATE Products
        SET 
            Stock = (
                SELECT ISNULL(SUM(Stock), 0) 
                FROM ProductVariants 
                WHERE ProductVariants.ProductID = Products.ProductID
            ),
            Sold = (
                SELECT ISNULL(SUM(Sold), 0) 
                FROM ProductVariants 
                WHERE ProductVariants.ProductID = Products.ProductID
            )
        FROM Products
        INNER JOIN inserted
        ON Products.ProductID = inserted.ProductID;
    END

    -- Tính tổng Stock và Sold khi xóa
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        UPDATE Products
        SET 
            Stock = (
                SELECT ISNULL(SUM(Stock), 0) 
                FROM ProductVariants 
                WHERE ProductVariants.ProductID = Products.ProductID
            ),
            Sold = (
                SELECT ISNULL(SUM(Sold), 0) 
                FROM ProductVariants 
                WHERE ProductVariants.ProductID = Products.ProductID
            )
        FROM Products
        INNER JOIN deleted
        ON Products.ProductID = deleted.ProductID;
    END
END;

GO


-- Thêm dữ liệu mẫu cho Users
INSERT INTO Users (FullName, Email, Phone, Password, Address, Birthday, Gender) VALUES
(N'Nguyễn Văn An', 'an.nguyen@gmail.com', '0901234567', '123456', N'123 Nguyễn Văn Cừ, Q.5, TP.HCM', '1990-01-15', 'Nam'),
(N'Trần Thị Bình', 'binh.tran@gmail.com', '0912345678', '123456', N'456 Lê Lợi, Q.1, TP.HCM', '1995-05-20', N'Nữ'),
(N'Lê Hoàng Cường', 'cuong.le@gmail.com', '0923456789', '123456', N'789 Cách Mạng Tháng 8, Q.3, TP.HCM', '1988-12-10', 'Nam'),
(N'Phạm Thị Dung', 'dung.pham@gmail.com', '0934567890', '123456', N'321 Võ Văn Tần, Q.3, TP.HCM', '1992-08-25', N'Nữ')

-- Thêm dữ liệu mẫu cho Categories
INSERT INTO Categories (CategoryName, IconUrl, Description) VALUES 
(N'Huawei', './assets/icons/smartphone.svg', N'Điện thoại Huawei chính hãng'),
(N'iPhone', './assets/icons/apple.svg', N'iPhone chính hãng VN/A'),
(N'Samsung', './assets/icons/samsung.svg', N'Điện thoại Samsung chính hãng'),
(N'Xiaomi', './assets/icons/xiaomi.svg', N'Điện thoại Xiaomi chính hãng'),
(N'OPPO', './assets/icons/smartphone.svg', N'Điện thoại OPPO chính hãng'),
(N'Vivo', './assets/icons/smartphone.svg', N'Điện thoại Vivo chính hãng')

-- Thêm dữ liệu mẫu cho Products
INSERT INTO Products (CategoryID, ProductName, Description, OriginalPrice, SalePrice, DefaultImageUrl, Stock) VALUES 
(2, N'iPhone 14 Pro Max', N'Màn hình Super Retina XDR 6.7 inch, chip A16 Bionic, camera 48MP', 34990000, 27990000, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg', 50),
(2, N'iPhone 14 Pro', N'Màn hình Super Retina XDR 6.1 inch, chip A16 Bionic, camera 48MP', 30990000, 25990000, 'https://cdn.tgdd.vn/Products/Images/42/247508/iphone-14-pro-tim-thumb-600x600.jpg', 45),
(2, N'iPhone 14', N'Màn hình Super Retina XDR 6.1 inch, chip A15 Bionic', 24990000, 19990000, 'https://cdn.tgdd.vn/Products/Images/42/240259/iphone-14-thumb-tim-600x600.jpg', 60),
(3, N'Samsung Galaxy S23 Ultra', N'Màn hình Dynamic AMOLED 2X 6.8 inch, Snapdragon 8 Gen 2', 31990000, 26990000, 'https://cdn.tgdd.vn/Products/Images/42/249948/samsung-galaxy-s23-ultra-tim-thumb-600x600.jpg', 40),
(3, N'Samsung Galaxy Z Fold4', N'Màn hình Dynamic AMOLED 2X 7.6 inch gập, Snapdragon 8+ Gen 1', 40990000, 34990000, 'https://cdn.tgdd.vn/Products/Images/42/250625/samsung-galaxy-z-fold4-xanh-thumb-600x600.jpg', 30),
(4, N'Xiaomi 13 Pro', N'Màn hình AMOLED 6.73 inch, Snapdragon 8 Gen 2', 24990000, 22990000, 'https://cdn.tgdd.vn/Products/Images/42/282903/xiaomi-13-pro-thumb-1-600x600.jpg', 35),
(5, N'OPPO Find X5 Pro', N'Màn hình AMOLED 6.7 inch, Snapdragon 8 Gen 1', 32990000, 24990000, 'https://cdn.tgdd.vn/Products/Images/42/250622/oppo-find-x5-pro-den-thumb-600x600.jpg', 25)

-- Thêm dữ liệu mẫu cho ProductVariants

-- Thêm dữ liệu mẫu cho ProductSpecs
INSERT INTO ProductSpecs (ProductID, SpecName, SpecValue) VALUES
(1, N'Màn hình', N'OLED 6.7 inch Super Retina XDR'),
(1, N'Chip', 'Apple A16 Bionic'),
(1, N'RAM', '6GB'),
(1, N'Pin', '4323 mAh'),
(1, N'Camera sau', N'Chính 48 MP & Phụ 12 MP, 12 MP'),
(1, N'Camera trước', '12 MP'),
(2, N'Màn hình', N'OLED 6.1 inch Super Retina XDR'),
(2, N'Chip', 'Apple A16 Bionic'),
(2, N'RAM', '6GB'),
(2, N'Pin', '3200 mAh'),
(4, N'Màn hình', N'Dynamic AMOLED 2X 6.8 inch'),
(4, N'Chip', 'Snapdragon 8 Gen 2'),
(4, N'RAM', '8GB'),
(4, N'Pin', '5000 mAh')

-- Thêm dữ liệu mẫu cho ProductImages
INSERT INTO ProductImages (ProductID, ImageUrl, IsMainImage) VALUES
(1, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg', 1),
(1, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-purple-1.jpg', 0),
(1, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-black-2.jpg', 0),
(2, 'https://cdn.tgdd.vn/Products/Images/42/247508/iphone-14-pro-tim-thumb-600x600.jpg', 1),
(2, 'https://cdn.tgdd.vn/Products/Images/42/247508/iphone-14-pro-silver-1.jpg', 0)

-- Thêm dữ liệu mẫu cho Orders
INSERT INTO Orders (UserID, Status, ShippingAddress, ShippingPhone, TotalAmount, Note) VALUES
(1, N'Đã giao', N'123 Nguyễn Văn Cừ, Q.5, TP.HCM', '0901234567', 27990000, N'Giao giờ hành chính'),
(2, N'Đang giao', N'456 Lê Lợi, Q.1, TP.HCM', '0912345678', 51980000, N'Giao buổi sáng'),
(3, N'Chờ xác nhận', N'789 Cách Mạng Tháng 8, Q.3, TP.HCM', '0923456789', 19990000, NULL),
(4, N'Đã hủy', N'321 Võ Văn Tần, Q.3, TP.HCM', '0934567890', 26990000, N'Đổi ý không mua')

INSERT INTO Orders (UserID, Status, ShippingAddress, ShippingPhone, TotalAmount, Note) VALUES
(5, N'Đã hủy', N'321 Võ Văn Tần, Q.3, TP.HCM', '0934567890', 26990000, N'Đổi ý không mua'),
(5, N'Đã giao', N'123 Nguyễn Văn Cừ, Q.5, TP.HCM', '0901234567', 27990000, N'Giao giờ hành chính')
-- Thêm dữ liệu mẫu cho OrderDetails
INSERT INTO OrderDetails (OrderID, ProductID, VariantID, Quantity, Price) VALUES
(1, 1, 1, 1, 27990000),
(2, 1, 2, 1, 30990000),
(2, 2, 5, 1, 25990000),
(3, 3, 7, 1, 19990000),
(4, 4, 9, 1, 26990000)


INSERT INTO ProductVariants (ProductID, Color, Storage, Price, Stock, VarImageUrl, Sold) VALUES
(1, N'Xanh than', '128', 27990000, 10, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg',2),
(1, N'Xanh than', '256', 30990000, 10, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg',3),
(2, N'Xanh than', '256', 30990000, 10, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg',3),
(3, N'Xanh than', '256', 30990000, 10, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg',3),
(2, N'Xanh than', '256', 30990000, 10, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg',3)
-- Thêm biến thể cho iPhone 14 Pro Max
INSERT INTO ProductVariants (ProductID, Color, Storage, Price, Stock, VarImageUrl) VALUES 
(1, N'Tím Deep Purple', '128', 27990000, 10, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg'),
(1, N'Tím Deep Purple', '256', 30990000, 10, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg'),
(1, N'Vàng Gold', '128', 28500000, 12, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg'),
(1, N'Vàng Gold', '256', 35500000, 8, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg'),
(1, N'Đen Space Black', '128', 27990000, 20, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg'),
(1, N'Đen Space Black', '256', 30990000, 15, 'https://cdn.tgdd.vn/Products/Images/42/251192/iphone-14-pro-max-vang-thumb-600x600.jpg');

-- Thêm biến thể cho iPhone 14 Pro
INSERT INTO ProductVariants (ProductID, Color, Storage, Price, Stock) VALUES 
(2, N'Tím Deep Purple', '128', 25990000, 20),
(2, N'Tím Deep Purple', '256', 28990000, 15),
(2, N'Bạc Silver', '128', 25990000, 18),
(2, N'Bạc Silver', '256', 28990000, 12);

-- Thêm biến thể cho iPhone 14
INSERT INTO ProductVariants (ProductID, Color, Storage, Price, Stock) VALUES 
(3, N'Xanh Blue', '128', 19990000, 25),
(3, N'Xanh Blue', '256', 22990000, 20),
(3, N'Đỏ Red', '128', 19990000, 22),
(3, N'Đỏ Red', '256', 22990000, 18);

-- Thêm biến thể cho Samsung Galaxy S23 Ultra
INSERT INTO ProductVariants (ProductID, Color, Storage, Price, Stock) VALUES 
(4, N'Kem Cream', '256', 26990000, 18),
(4, N'Kem Cream', '512', 29990000, 12),
(4, N'Đen Phantom Black', '256', 26990000, 15),
(4, N'Đen Phantom Black', '512', 29990000, 10);

-- Thêm biến thể cho Samsung Galaxy Z Fold4
INSERT INTO ProductVariants (ProductID, Color, Storage, Price, Stock) VALUES 
(5, N'Đen Phantom Black', '256', 34990000, 10),
(5, N'Đen Phantom Black', '512', 37990000, 8),
(5, N'Xanh Phantom Green', '256', 34990000, 12),
(5, N'Xanh Phantom Green', '512', 37990000, 7);

-- Thêm biến thể cho Xiaomi 13 Pro
INSERT INTO ProductVariants (ProductID, Color, Storage, Price, Stock) VALUES 
(6, N'Đen Ceramic Black', '256', 22990000, 20),
(6, N'Trắng Ceramic White', '256', 22990000, 18);

-- Thêm biến thể cho OPPO Find X5 Pro
INSERT INTO ProductVariants (ProductID, Color, Storage, Price, Stock) VALUES 
(7, N'Đen Glaze Black', '256', 24990000, 15),
(7, N'Trắng Ceramic White', '256', 24990000, 12)

select * from Products
  SELECT TOP 6 
                                ProductID,
                                ProductName,
                                DefaultImageUrl,
                                OriginalPrice,
                                SalePrice
                            FROM Products
                            WHERE IsActive = 1
                            ORDER BY Sold DESC