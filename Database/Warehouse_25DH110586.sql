
CREATE DATABASE Warehouse_25DH110586;
GO

USE Warehouse_25DH110586;
GO

CREATE TABLE Products
(
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL
);
GO

INSERT INTO Products (ProductName, Category, Price, Quantity)
VALUES
(N'Bút bi - Ph??ng Trinh', N'V?n phòng ph?m - Ph??ng Trinh', 15000, 20),
(N'S? tay - Ph??ng Trinh', N'V?n phòng ph?m - Ph??ng Trinh', 90000, 10),
(N'Bàn phím - Ph??ng Trinh', N'Thi?t b? công ngh? - Ph??ng Trinh', 200000, 8),
(N'Chu?t máy tính - Ph??ng Trinh', N'Thi?t b? công ngh? - Ph??ng Trinh', 350000, 12),
(N'Tai nghe - Ph??ng Trinh', N'Thi?t b? công ngh? - Ph??ng Trinh', 500000, 5);
GO

SELECT * FROM Products;
