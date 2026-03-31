USE device_management_db;
GO

INSERT INTO users (name, role, location)
SELECT * FROM (VALUES
('horia', 'admin', 'Cluj-Napoca'),
('alex', 'user', 'Bucharest'),
('maria', 'manager', 'Timisoara'),
('andrei', 'user', 'Iasi'),
('elena', 'support', 'Brasov'),
('ion', 'user', 'Constanta'),
('ana', 'manager', 'Oradea'),
('george', 'support', 'Sibiu'),
('dan', 'user', 'Arad'),
('paula', 'user', 'Pitesti'),
('mihai', 'admin', 'Ploiesti'),
('cristina', 'manager', 'Bacau'),
('robert', 'user', 'Suceava'),
('bianca', 'support', 'Targu Mures'),
('florin', 'user', 'Buzau'),
('diana', 'manager', 'Galati'),
('vlad', 'user', 'Braila'),
('simona', 'support', 'Deva'),
('alin', 'user', 'Resita'),
('irina', 'manager', 'Satu Mare')
) AS v(name, role, location)
WHERE NOT EXISTS (
    SELECT 1 FROM users u WHERE u.name = v.name
);
GO

INSERT INTO devices (name, type, operating_system, os_version, processor, ram, description, manufacturer)
SELECT * FROM (VALUES
('Laptop-HP-01', 'Laptop', 'Windows', '11', 'Intel i7', '16GB', 'Work laptop', 'HP'),
('Laptop-Dell-02', 'Laptop', 'Windows', '11', 'Intel i5', '8GB', 'Office laptop', 'Dell'),
('Laptop-Lenovo-03', 'Laptop', 'Windows', '10', 'Intel i5', '8GB', 'Student laptop', 'Lenovo'),
('MacBook-Pro-01', 'Laptop', 'macOS', 'Sonoma', 'Apple M2', '16GB', 'Dev machine', 'Apple'),
('MacBook-Air-02', 'Laptop', 'macOS', 'Ventura', 'Apple M1', '8GB', 'Lightweight laptop', 'Apple'),
('Desktop-Gaming-01', 'Desktop', 'Windows', '11', 'AMD Ryzen 9', '32GB', 'Gaming PC', 'Custom'),
('Desktop-Gaming-02', 'Desktop', 'Windows', '10', 'Intel i9', '32GB', 'High-end gaming', 'Custom'),
('Desktop-Office-01', 'Desktop', 'Windows', '10', 'Intel i3', '8GB', 'Office PC', 'Custom'),
('Desktop-Office-02', 'Desktop', 'Windows', '11', 'Intel i5', '16GB', 'Workstation', 'Custom'),
('Server-01', 'Server', 'Linux', 'Ubuntu 22.04', 'Intel Xeon', '64GB', 'Production server', 'Dell'),
('Server-02', 'Server', 'Linux', 'Debian 11', 'AMD EPYC', '128GB', 'Backup server', 'HP'),
('Server-03', 'Server', 'Linux', 'CentOS 7', 'Intel Xeon', '32GB', 'Legacy server', 'Lenovo'),
('Tablet-Samsung-01', 'Tablet', 'Android', '13', 'Snapdragon 8', '8GB', 'Portable tablet', 'Samsung'),
('Tablet-Samsung-02', 'Tablet', 'Android', '12', 'Snapdragon 7', '6GB', 'Company tablet', 'Samsung'),
('iPad-01', 'Tablet', 'iOS', '17', 'Apple A14', '6GB', 'Apple tablet', 'Apple'),
('iPad-02', 'Tablet', 'iOS', '16', 'Apple A13', '4GB', 'Old iPad', 'Apple'),
('Phone-iPhone-01', 'Phone', 'iOS', '17', 'Apple A16', '6GB', 'Company phone', 'Apple'),
('Phone-iPhone-02', 'Phone', 'iOS', '16', 'Apple A15', '6GB', 'Backup phone', 'Apple'),
('Phone-Samsung-01', 'Phone', 'Android', '13', 'Snapdragon 8 Gen 1', '8GB', 'Android phone', 'Samsung'),
('Phone-Samsung-02', 'Phone', 'Android', '12', 'Snapdragon 888', '8GB', 'Android device', 'Samsung'),
('Laptop-Asus-01', 'Laptop', 'Windows', '11', 'Intel i7', '16GB', 'Developer laptop', 'Asus'),
('Laptop-Acer-01', 'Laptop', 'Windows', '10', 'Intel i5', '8GB', 'Office use', 'Acer'),
('Desktop-Design-01', 'Desktop', 'Windows', '11', 'AMD Ryzen 7', '32GB', 'Design workstation', 'Custom'),
('Server-Cloud-01', 'Server', 'Linux', 'Ubuntu 20.04', 'AMD EPYC', '256GB', 'Cloud node', 'Dell'),
('Tablet-Lenovo-01', 'Tablet', 'Android', '11', 'MediaTek', '4GB', 'Cheap tablet', 'Lenovo'),
('Phone-Google-01', 'Phone', 'Android', '14', 'Tensor G3', '8GB', 'Pixel phone', 'Google'),
('Laptop-MS-01', 'Laptop', 'Windows', '11', 'Intel i7', '16GB', 'Surface laptop', 'Microsoft'),
('Desktop-Mini-01', 'Desktop', 'Windows', '11', 'Intel i5', '8GB', 'Mini PC', 'Intel'),
('Server-Test-01', 'Server', 'Linux', 'Ubuntu 22.04', 'Intel Xeon', '16GB', 'Testing server', 'Dell'),
('Laptop-HP-Backup', 'Laptop', 'Windows', '10', 'Intel i3', '4GB', 'Old backup laptop', 'HP')
) AS v(name, type, operating_system, os_version, processor, ram, description, manufacturer)
WHERE NOT EXISTS (
    SELECT 1 FROM devices d WHERE d.name = v.name
);
GO