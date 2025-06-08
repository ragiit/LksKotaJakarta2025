-- ================================================
-- Create (or recreate) the GSA database
-- ================================================
IF DB_ID('GSA') IS NOT NULL
BEGIN
	ALTER DATABASE GSA SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE GSA;
END
GO

CREATE DATABASE GSA;
GO

USE GSA;
GO

-- ================================================
-- 1. Users
-- ================================================
IF OBJECT_ID('dbo.Users','U') IS NOT NULL DROP TABLE dbo.Users;
GO

CREATE TABLE dbo.Users (
    id             INT IDENTITY(1,1)    PRIMARY KEY,
    name           NVARCHAR(255)        NOT NULL,
    username       NVARCHAR(100)        NOT NULL UNIQUE,
    email          NVARCHAR(255)        NOT NULL UNIQUE,
    password_hash  NVARCHAR(255)        NOT NULL,
    role           NVARCHAR(10)         NOT NULL
        CONSTRAINT CK_Users_Role CHECK (role IN ('admin','student')),
    created_at     DATETIME2            NOT NULL DEFAULT SYSUTCDATETIME(),
    updated_at     DATETIME2            NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- ================================================
-- 2. Courses
-- ================================================
IF OBJECT_ID('dbo.Courses','U') IS NOT NULL DROP TABLE dbo.Courses;
GO

CREATE TABLE dbo.Courses (
    id            INT IDENTITY(1,1)   PRIMARY KEY,
    title         NVARCHAR(255)       NOT NULL,
    description   NVARCHAR(MAX)       NOT NULL,
    price         DECIMAL(10,2)       NOT NULL,
    duration      INT                 NOT NULL,  -- duration in minutes
    created_at    DATETIME2           NOT NULL DEFAULT SYSUTCDATETIME(),
    updated_at    DATETIME2           NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- ================================================
-- 3. Modules
-- ================================================
IF OBJECT_ID('dbo.Modules','U') IS NOT NULL DROP TABLE dbo.Modules;
GO

CREATE TABLE dbo.Modules (
    id         INT IDENTITY(1,1)  PRIMARY KEY,
    course_id  INT                NOT NULL
        CONSTRAINT FK_Modules_Courses FOREIGN KEY REFERENCES dbo.Courses(id) ON DELETE CASCADE,
    title      NVARCHAR(255)      NOT NULL,
    content    NVARCHAR(MAX)      
);
GO

-- ================================================
-- 4. Coupons
-- ================================================
IF OBJECT_ID('dbo.Coupons','U') IS NOT NULL DROP TABLE dbo.Coupons;
GO

CREATE TABLE dbo.Coupons (
    id            INT IDENTITY(1,1)   PRIMARY KEY,
    code          NVARCHAR(50)        NOT NULL UNIQUE,
    discount_pct  DECIMAL(5,2)        NOT NULL,  -- e.g. 15.00 = 15%
    quota         INT                 NOT NULL,
    expiry_date   DATETIME2           NOT NULL,
    created_at    DATETIME2           NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- ================================================
-- 5. Purchases
-- ================================================
IF OBJECT_ID('dbo.Purchases','U') IS NOT NULL DROP TABLE dbo.Purchases;
GO

CREATE TABLE dbo.Purchases (
    id              INT IDENTITY(1,1)    PRIMARY KEY,
    user_id         INT                  NOT NULL
        CONSTRAINT FK_Purchases_Users   FOREIGN KEY REFERENCES dbo.Users(id),
    course_id       INT                  NOT NULL
        CONSTRAINT FK_Purchases_Courses FOREIGN KEY REFERENCES dbo.Courses(id),
    coupon_id       INT                  NULL
        CONSTRAINT FK_Purchases_Coupons FOREIGN KEY REFERENCES dbo.Coupons(id),
    price_paid      DECIMAL(10,2)        NOT NULL,
    payment_method  NVARCHAR(50)         NOT NULL,  -- e.g. 'credit_card', 'paypal'
    purchased_at    DATETIME2            NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

USE GSA;
-- Sample Users
INSERT INTO dbo.Users (name, username, email, password_hash, role) VALUES ('Alice Johnson', 'alicej', 'alice@example.com', '46708f23d682fef9aa996ecbb139bfb6c9ffdc039905ad6ad5c85a88b9411d97', 'student');
INSERT INTO dbo.Users (name, username, email, password_hash, role) VALUES ('Bob Smith', 'bobsmith', 'bob@example.com', '46708f23d682fef9aa996ecbb139bfb6c9ffdc039905ad6ad5c85a88b9411d97', 'student');
INSERT INTO dbo.Users (name, username, email, password_hash, role) VALUES ('Charlie Lee', 'charliel', 'charlie@example.com', '46708f23d682fef9aa996ecbb139bfb6c9ffdc039905ad6ad5c85a88b9411d97', 'student');
INSERT INTO dbo.Users (name, username, email, password_hash, role) VALUES ('Diana Prince', 'dianap', 'diana@example.com', '46708f23d682fef9aa996ecbb139bfb6c9ffdc039905ad6ad5c85a88b9411d97', 'student');
INSERT INTO dbo.Users (name, username, email, password_hash, role) VALUES ('Ethan Hunt', 'ethanh', 'ethan@example.com', '46708f23d682fef9aa996ecbb139bfb6c9ffdc039905ad6ad5c85a88b9411d97', 'student');
INSERT INTO dbo.Users (name, username, email, password_hash, role) VALUES ('Admin User', 'admin', 'admin@growthseeker.com', '46708f23d682fef9aa996ecbb139bfb6c9ffdc039905ad6ad5c85a88b9411d97', 'admin');

USE GSA;
-- Sample Courses and Modules
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Introduction to C#', 'Learn the basics of C# programming.', 99.99, 600);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Advanced Web Development', 'Deep dive into modern web technologies.', 149.99, 900);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Mastering JavaScript', 'A comprehensive course on JavaScript.', 129.99, 900);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Full-Stack Development with React and Node.js', 'Build modern full-stack web applications.', 139.99, 840);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Building REST APIs with Java Spring', 'Create robust RESTful services using Spring Boot.', 149.99, 720);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Database Fundamentals with PostgreSQL', 'Master database design and SQL queries.', 89.99, 480);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('AWS Cloud Infrastructure Basics', 'Learn to deploy and manage applications on AWS.', 119.99, 600);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Data Engineering with Python', 'Process and transform large datasets.', 139.99, 750);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Firebase for Mobile Apps', 'Build and deploy mobile backends with Firebase.', 109.99, 540);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Nest.js Deep Dive', 'Develop scalable server-side applications with Nest.js.', 129.99, 660);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Secure Web Authentication with JWT', 'Implement secure JWT-based authentication.', 99.99, 480);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Advanced CSS and Tailwind', 'Design modern interfaces with CSS & Tailwind.', 89.99, 360);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('DevOps Essentials with Docker & Kubernetes', 'Containerize and orchestrate applications.', 149.99, 780);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Real-time Applications with WebSockets', 'Build real-time features using WebSocket technology.', 119.99, 600);
INSERT INTO dbo.Courses (title, description, price, duration) VALUES ('Unit Testing in Backend with Jest and Mocha', 'Write and run backend tests effectively.', 99.99, 420);

INSERT INTO Modules (course_id, title, content) VALUES (1, 'Module 1: Iterate Integrated E-Markets', 'This module covers the topic of module 1: iterate integrated e-markets in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (1, 'Module 2: Integrate Back-End Mindshare', 'This module covers the topic of module 2: integrate back-end mindshare in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (1, 'Module 3: Synthesize Wireless Content', 'This module covers the topic of module 3: synthesize wireless content in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (1, 'Module 4: Syndicate Synergistic Applications', 'This module covers the topic of module 4: syndicate synergistic applications in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (1, 'Module 5: Productize Killer Mindshare', 'This module covers the topic of module 5: productize killer mindshare in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (1, 'Module 6: Enable Front-End Channels', 'This module covers the topic of module 6: enable front-end channels in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (1, 'Module 7: Engineer Mission-Critical E-Business', 'This module covers the topic of module 7: engineer mission-critical e-business in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (1, 'Module 8: Exploit Dot-Com Users', 'This module covers the topic of module 8: exploit dot-com users in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (1, 'Module 9: Unleash B2B Content', 'This module covers the topic of module 9: unleash b2b content in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (2, 'Module 1: Transform Intuitive Methodologies', 'This module covers the topic of module 1: transform intuitive methodologies in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (2, 'Module 2: Facilitate Collaborative Schemas', 'This module covers the topic of module 2: facilitate collaborative schemas in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (2, 'Module 3: Transform Synergistic E-Commerce', 'This module covers the topic of module 3: transform synergistic e-commerce in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (2, 'Module 4: Harness Best-Of-Breed Schemas', 'This module covers the topic of module 4: harness best-of-breed schemas in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (2, 'Module 5: Syndicate Visionary Deliverables', 'This module covers the topic of module 5: syndicate visionary deliverables in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (2, 'Module 6: Morph Leading-Edge Schemas', 'This module covers the topic of module 6: morph leading-edge schemas in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (2, 'Module 7: Redefine Value-Added Infrastructures', 'This module covers the topic of module 7: redefine value-added infrastructures in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (3, 'Module 1: Drive Wireless Web Services', 'This module covers the topic of module 1: drive wireless web services in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (3, 'Module 2: Whiteboard Clicks-And-Mortar E-Business', 'This module covers the topic of module 2: whiteboard clicks-and-mortar e-business in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (3, 'Module 3: Synthesize Open-Source Action-Items', 'This module covers the topic of module 3: synthesize open-source action-items in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (3, 'Module 4: Drive World-Class Partnerships', 'This module covers the topic of module 4: drive world-class partnerships in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (3, 'Module 5: Aggregate Magnetic Action-Items', 'This module covers the topic of module 5: aggregate magnetic action-items in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (4, 'Module 1: Architect Front-End Systems', 'This module covers the topic of module 1: architect front-end systems in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (4, 'Module 2: Evolve Integrated Vortals', 'This module covers the topic of module 2: evolve integrated vortals in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (4, 'Module 3: Maximize Web-Enabled Technologies', 'This module covers the topic of module 3: maximize web-enabled technologies in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (4, 'Module 4: Reinvent Dynamic Roi', 'This module covers the topic of module 4: reinvent dynamic roi in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (4, 'Module 5: Integrate Back-End Info-Mediaries', 'This module covers the topic of module 5: integrate back-end info-mediaries in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (4, 'Module 6: Optimize E-Business Methodologies', 'This module covers the topic of module 6: optimize e-business methodologies in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (4, 'Module 7: Engineer End-To-End Eyeballs', 'This module covers the topic of module 7: engineer end-to-end eyeballs in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (5, 'Module 1: Re-Contextualize Robust Infrastructures', 'This module covers the topic of module 1: re-contextualize robust infrastructures in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (5, 'Module 2: Productize Impactful Deliverables', 'This module covers the topic of module 2: productize impactful deliverables in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (5, 'Module 3: Empower Web-Enabled E-Tailers', 'This module covers the topic of module 3: empower web-enabled e-tailers in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (6, 'Module 1: Incentivize Vertical Niches', 'This module covers the topic of module 1: incentivize vertical niches in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (6, 'Module 2: Embrace Magnetic Relationships', 'This module covers the topic of module 2: embrace magnetic relationships in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (6, 'Module 3: Generate Frictionless Supply-Chains', 'This module covers the topic of module 3: generate frictionless supply-chains in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (7, 'Module 1: Embrace Dynamic Metrics', 'This module covers the topic of module 1: embrace dynamic metrics in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (7, 'Module 2: E-Enable 24/7 Interfaces', 'This module covers the topic of module 2: e-enable 24/7 interfaces in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (7, 'Module 3: Evolve Out-Of-The-Box Initiatives', 'This module covers the topic of module 3: evolve out-of-the-box initiatives in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (7, 'Module 4: Integrate Vertical Solutions', 'This module covers the topic of module 4: integrate vertical solutions in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (7, 'Module 5: Reinvent 24/7 Info-Mediaries', 'This module covers the topic of module 5: reinvent 24/7 info-mediaries in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (8, 'Module 1: Architect Leading-Edge Methodologies', 'This module covers the topic of module 1: architect leading-edge methodologies in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (8, 'Module 2: Utilize Cutting-Edge E-Business', 'This module covers the topic of module 2: utilize cutting-edge e-business in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (8, 'Module 3: Transform Back-End Partnerships', 'This module covers the topic of module 3: transform back-end partnerships in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (8, 'Module 4: Enhance Intuitive Experiences', 'This module covers the topic of module 4: enhance intuitive experiences in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (8, 'Module 5: Monetize Cross-Media E-Commerce', 'This module covers the topic of module 5: monetize cross-media e-commerce in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (9, 'Module 1: Reinvent Leading-Edge Mindshare', 'This module covers the topic of module 1: reinvent leading-edge mindshare in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (9, 'Module 2: Target Revolutionary Metrics', 'This module covers the topic of module 2: target revolutionary metrics in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (9, 'Module 3: Transform Impactful Solutions', 'This module covers the topic of module 3: transform impactful solutions in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (9, 'Module 4: Morph Synergistic Info-Mediaries', 'This module covers the topic of module 4: morph synergistic info-mediaries in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (9, 'Module 5: E-Enable Frictionless Methodologies', 'This module covers the topic of module 5: e-enable frictionless methodologies in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (9, 'Module 6: Architect Leading-Edge Methodologies', 'This module covers the topic of module 6: architect leading-edge methodologies in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (9, 'Module 7: Leverage Granular Supply-Chains', 'This module covers the topic of module 7: leverage granular supply-chains in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (10, 'Module 1: Mesh Dot-Com Initiatives', 'This module covers the topic of module 1: mesh dot-com initiatives in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (10, 'Module 2: Expedite Visionary Web Services', 'This module covers the topic of module 2: expedite visionary web services in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (10, 'Module 3: Reinvent Value-Added Info-Mediaries', 'This module covers the topic of module 3: reinvent value-added info-mediaries in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (10, 'Module 4: Envisage Turnkey Action-Items', 'This module covers the topic of module 4: envisage turnkey action-items in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (10, 'Module 5: Synergize Market-Leading Methodologies', 'This module covers the topic of module 5: synergize market-leading methodologies in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (10, 'Module 6: Reinvent Dynamic Infrastructures', 'This module covers the topic of module 6: reinvent dynamic infrastructures in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (11, 'Module 1: Enable Scalable Action-Items', 'This module covers the topic of module 1: enable scalable action-items in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (11, 'Module 2: Deploy Visionary Web Services', 'This module covers the topic of module 2: deploy visionary web services in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (11, 'Module 3: Engineer Robust Methodologies', 'This module covers the topic of module 3: engineer robust methodologies in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (11, 'Module 4: Productize Cutting-Edge Frameworks', 'This module covers the topic of module 4: productize cutting-edge frameworks in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (11, 'Module 5: Facilitate High-Level Metrics', 'This module covers the topic of module 5: facilitate high-level metrics in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (12, 'Module 1: Leverage Synergistic Technologies', 'This module covers the topic of module 1: leverage synergistic technologies in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (12, 'Module 2: Build Scalable Partnerships', 'This module covers the topic of module 2: build scalable partnerships in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (12, 'Module 3: Optimize Cross-Functional Tools', 'This module covers the topic of module 3: optimize cross-functional tools in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (12, 'Module 4: Harness World-Class Synergy', 'This module covers the topic of module 4: harness world-class synergy in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (12, 'Module 5: Reinvent Visionary E-Commerce', 'This module covers the topic of module 5: reinvent visionary e-commerce in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (12, 'Module 6: Enable Seamless Integrations', 'This module covers the topic of module 6: enable seamless integrations in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (13, 'Module 1: Architect Seamless E-Business', 'This module covers the topic of module 1: architect seamless e-business in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (13, 'Module 2: Build Scalable Enterprise Solutions', 'This module covers the topic of module 2: build scalable enterprise solutions in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (13, 'Module 3: Integrate Robust Cloud Platforms', 'This module covers the topic of module 3: integrate robust cloud platforms in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (13, 'Module 4: Drive Cutting-Edge Metrics', 'This module covers the topic of module 4: drive cutting-edge metrics in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (13, 'Module 5: Transform Digital Strategies', 'This module covers the topic of module 5: transform digital strategies in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (14, 'Module 1: Optimize Scalable Web Frameworks', 'This module covers the topic of module 1: optimize scalable web frameworks in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (14, 'Module 2: Harness Real-Time Analytics', 'This module covers the topic of module 2: harness real-time analytics in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (14, 'Module 3: Leverage Cutting-Edge Solutions', 'This module covers the topic of module 3: leverage cutting-edge solutions in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (14, 'Module 4: Innovate Seamless User Experiences', 'This module covers the topic of module 4: innovate seamless user experiences in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (14, 'Module 5: Reinvent Cross-Platform Systems', 'This module covers the topic of module 5: reinvent cross-platform systems in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (14, 'Module 6: Build Scalable Cloud Platforms', 'This module covers the topic of module 6: build scalable cloud platforms in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (15, 'Module 1: Engineer Data-Driven Systems', 'This module covers the topic of module 1: engineer data-driven systems in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (15, 'Module 2: Automate Cloud Infrastructure', 'This module covers the topic of module 2: automate cloud infrastructure in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (15, 'Module 3: Innovate Backend Systems', 'This module covers the topic of module 3: innovate backend systems in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (15, 'Module 4: Manage Large-Scale Databases', 'This module covers the topic of module 4: manage large-scale databases in detail.');
INSERT INTO Modules (course_id, title, content) VALUES (15, 'Module 5: Optimize Serverless Solutions', 'This module covers the topic of module 5: optimize serverless solutions in detail.');


-- Sample Coupons
INSERT INTO dbo.Coupons (code, discount_pct, quota, expiry_date) VALUES ('SPRINGSALE', 15.0, 100, '2025-04-30T00:00:00Z');
INSERT INTO dbo.Coupons (code, discount_pct, quota, expiry_date) VALUES ('SAVE20', 20.0, 50, '2025-04-24T00:00:00Z');
INSERT INTO dbo.Coupons (code, discount_pct, quota, expiry_date) VALUES ('FLASH10', 10.0, 10, '2025-04-14T00:00:00Z');
INSERT INTO dbo.Coupons (code, discount_pct, quota, expiry_date) VALUES ('WELCOME5', 5.0, 1000, '2026-04-19T00:00:00Z');
INSERT INTO dbo.Coupons (code, discount_pct, quota, expiry_date) VALUES ('HALFOFF', 50.0, 5, '2025-04-20T00:00:00Z');
INSERT INTO dbo.Coupons (code, discount_pct, quota, expiry_date) VALUES ('FREEMODULE', 100.0, 2, '2025-05-19T00:00:00Z');
INSERT INTO dbo.Coupons (code, discount_pct, quota, expiry_date) VALUES ('LIMITED25', 25.0, 0, '2025-06-18T00:00:00Z');

-- Sample Purchases
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (2, 9, 6, 0.0, 'paypal', '2025-03-22T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (3, 5, NULL, 185.46, 'debit_card', '2025-01-25T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (2, 12, NULL, 50.27, 'paypal', '2025-04-03T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (1, 14, 2, 82.37, 'paypal', '2025-02-23T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (4, 6, 3, 141.46, 'debit_card', '2025-03-04T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (1, 3, NULL, 164.54, 'debit_card', '2025-03-10T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (6, 5, 3, 166.91, 'paypal', '2025-03-20T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (6, 4, NULL, 156.4, 'paypal', '2025-04-06T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (2, 4, NULL, 156.4, 'credit_card', '2025-04-09T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (5, 13, NULL, 152.84, 'debit_card', '2025-03-06T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (4, 11, NULL, 112.62, 'paypal', '2025-02-18T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (1, 5, NULL, 185.46, 'debit_card', '2025-03-19T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (2, 14, NULL, 102.96, 'debit_card', '2025-04-18T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (1, 8, NULL, 177.82, 'paypal', '2025-04-01T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (6, 15, 6, 0.0, 'credit_card', '2025-03-09T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (5, 9, NULL, 105.55, 'debit_card', '2025-01-26T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (2, 2, NULL, 52.0, 'paypal', '2025-04-02T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (1, 2, NULL, 52.0, 'credit_card', '2025-02-14T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (5, 14, NULL, 102.96, 'credit_card', '2025-04-11T00:00:00Z');
INSERT INTO dbo.Purchases (user_id, course_id, coupon_id, price_paid, payment_method, purchased_at) VALUES (1, 7, NULL, 154.14, 'debit_card', '2025-02-11T00:00:00Z');