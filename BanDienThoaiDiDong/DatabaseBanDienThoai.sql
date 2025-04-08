-- Tạo database
CREATE DATABASE BanDienThoaiDiDong;
USE BanDienThoaiDiDong;

-- Bảng Users
CREATE TABLE Users (
    UserID INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Phone VARCHAR(15) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL,
    Address VARCHAR(200),
    Birthday DATE,
    Gender VARCHAR(10),
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    Role VARCHAR(20) DEFAULT 'User',
    IsActive BOOLEAN DEFAULT TRUE
);

-- Bảng Categories
CREATE TABLE Categories (
    CategoryID INT AUTO_INCREMENT PRIMARY KEY,
    CategoryName VARCHAR(50) NOT NULL,
    IconUrl VARCHAR(200),
    Description VARCHAR(500),
    IsActive BOOLEAN DEFAULT TRUE
);

-- Bảng Products
CREATE TABLE Products (
    ProductID INT AUTO_INCREMENT PRIMARY KEY,
    CategoryID INT,
    ProductName VARCHAR(200) NOT NULL,
    Description TEXT,
    OriginalPrice DECIMAL(18,0) NOT NULL,
    SalePrice DECIMAL(18,0) NOT NULL,
    DefaultImageUrl VARCHAR(500),
    Stock INT DEFAULT 0,
    Sold INT DEFAULT 0,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Bảng ProductVariants
CREATE TABLE ProductVariants (
    VariantID INT AUTO_INCREMENT PRIMARY KEY,
    ProductID INT,
    Color VARCHAR(50),
    ColorCode VARCHAR(10),
    Storage VARCHAR(50),
    Price DECIMAL(18,0),
    Stock INT DEFAULT 0,
    Sold INT DEFAULT 0,
    VarImageUrl VARCHAR(500),
    IsActive BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Bảng ProductSpecs
CREATE TABLE ProductSpecs (
    SpecID INT AUTO_INCREMENT PRIMARY KEY,
    ProductID INT,
    SpecName VARCHAR(100),
    SpecValue VARCHAR(200),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Bảng Orders
CREATE TABLE Orders (
    OrderID INT AUTO_INCREMENT PRIMARY KEY,
    UserID INT,
    OrderDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(50) DEFAULT 'Chờ xác nhận',
    ShippingAddress VARCHAR(200),
    ShippingPhone VARCHAR(15),
    ShippingFee DECIMAL(18,2),
    TotalAmount DECIMAL(18,0),
    Note VARCHAR(500),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Bảng OrderDetails
CREATE TABLE OrderDetails (
    OrderDetailID INT AUTO_INCREMENT PRIMARY KEY,
    OrderID INT,
    ProductID INT,
    VariantID INT,
    Quantity INT,
    Price DECIMAL(18,0),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (VariantID) REFERENCES ProductVariants(VariantID)
);

-- Bảng ProductImages
CREATE TABLE ProductImages (
    ImageID INT AUTO_INCREMENT PRIMARY KEY,
    ProductID INT,
    ImageUrl VARCHAR(500),
    IsMainImage BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Bảng CartItems
CREATE TABLE CartItems (
    CartItemID INT AUTO_INCREMENT PRIMARY KEY,
    UserID INT NOT NULL,
    ProductID INT NOT NULL,
    VariantID INT,
    Quantity INT DEFAULT 1,
    DateAdded DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (VariantID) REFERENCES ProductVariants(VariantID)
);

-- Bảng thống kê
CREATE TABLE SystemStatistics (
    StatID INT AUTO_INCREMENT PRIMARY KEY,
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
    LastUpdated DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Trigger cập nhật Stock và Sold trong Products khi ProductVariants thay đổi
DELIMITER $$
CREATE TRIGGER trg_UpdateProductStockSold
AFTER INSERT ON ProductVariants
FOR EACH ROW
BEGIN
    UPDATE Products
    SET Stock = (SELECT IFNULL(SUM(Stock), 0) FROM ProductVariants WHERE ProductID = NEW.ProductID),
        Sold = (SELECT IFNULL(SUM(Sold), 0) FROM ProductVariants WHERE ProductID = NEW.ProductID)
    WHERE ProductID = NEW.ProductID;
END$$

DELIMITER ;
