-- Create tables
CREATE TABLE category (
    id INT IDENTITY(1,1) PRIMARY KEY,
    category NVARCHAR(100) NOT NULL
);

CREATE TABLE status (
    id INT IDENTITY(1,1) PRIMARY KEY,
    status NVARCHAR(50) NOT NULL
);

CREATE TABLE personals (
    id INT IDENTITY(1,1) PRIMARY KEY,
    fio NVARCHAR(255) NOT NULL,
    staff NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL,
    password_hash NVARCHAR(255) NOT NULL,
    role NVARCHAR(20) NOT NULL DEFAULT 'IT_STAFF'
);

CREATE TABLE client (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    address NVARCHAR(255) NOT NULL,
    telephone NVARCHAR(20) NULL,
    email NVARCHAR(100) NOT NULL,
    password_hash NVARCHAR(255) NOT NULL,
    role NVARCHAR(20) NOT NULL DEFAULT 'CLIENT'
);

CREATE TABLE task (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX) NULL,
    personal_id INT NULL,
    client_id INT NOT NULL,
    date_created DATETIME NOT NULL DEFAULT GETDATE(),
    date_closed DATETIME NULL,
    category_id INT NOT NULL,
    status_id INT NOT NULL,
    FOREIGN KEY (personal_id) REFERENCES personal (id),
    FOREIGN KEY (client_id) REFERENCES client (id),
    FOREIGN KEY (category_id) REFERENCES category (id),
    FOREIGN KEY (status_id) REFERENCES status (id)
);

-- Insert initial data
INSERT INTO status (status) VALUES 
('Новая'), 
('В работе'), 
('Ожидает ответа клиента'),
('Завершена');

INSERT INTO category (category) VALUES 
('Техническая проблема'), 
('Установка ПО'), 
('Настройка оборудования'),
('Сетевая проблема'),
('Другое');

-- Insert sample IT staff member with SHA-256 hashed password 'admin123'
INSERT INTO personals (fio, staff, email, password_hash, role)
VALUES ('Иванов Иван Иванович', 'Системный администратор', 'admin@example.com', 
        CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', 'admin123'), 2), 'IT_STAFF');