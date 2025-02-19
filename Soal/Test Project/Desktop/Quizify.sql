CREATE DATABASE [Quizify_DB]
GO
USE [Quizify_DB]
GO

CREATE TABLE [User]
(
	[ID]					INT					NOT NULL	PRIMARY KEY	IDENTITY(1, 1),
	[FullName]				VARCHAR(200)		NOT NULL,
	[ExamCard]				VARCHAR(200)		NOT NULL,
	[Email]					VARCHAR(200),
	[Role]					CHAR(1)				NOT NULL,		-- (1) Admin, (2) Student
	[Gender]				CHAR(1)				NOT NULL,			-- (1) Male, (2) Female
	[BirthDate]				DATE				NOT NULL,
	[Password]				VARCHAR(200)		NOT NULL,
	[IsActive]				BIT					NOT NULL
);

CREATE TABLE [Subject]
(
	[ID]					INT					NOT NULL	PRIMARY KEY	IDENTITY(1, 1),
	[Name]					VARCHAR(200)		NOT NULL,
	[Time]					INT					NOT NULL,	-- Minutes
);

CREATE TABLE [Question]
(
	[ID]					INT					NOT NULL	PRIMARY KEY	IDENTITY(1, 1),
	[SubjectID]				INT					NOT NULL,
	[Question]				TEXT				NOT NULL,	-- Image after coma (,)
	[OptionA]				VARCHAR(200)		NOT NULL,
	[OptionB]				VARCHAR(200)		NOT NULL,
	[OptionC]				VARCHAR(200)		NOT NULL,
	[OptionD]				VARCHAR(200)		NOT NULL,
	[CorrectAnswer]			VARCHAR(200)		NOT NULL,
	CONSTRAINT FK_Question_Subject_SubjectID FOREIGN KEY ([SubjectID]) REFERENCES [Subject]([ID])
);

CREATE TABLE [Participant]
(
	[ID]					INT					NOT NULL	PRIMARY KEY	IDENTITY(1, 1),
	[UserID]				INT					NOT NULL,
	[SubjectID]				INT					NOT NULL,
	[Date]					DATE				NOT NULL,
	[TimeTaken]				INT					NOT NULL,
	CONSTRAINT FK_SubjectParticipant_Subject_SubjectID FOREIGN KEY ([SubjectID]) REFERENCES [Subject]([ID]),
	CONSTRAINT FK_UserParticipant_User_UserID FOREIGN KEY ([UserID]) REFERENCES [User]([ID])
);

CREATE TABLE [ParticipantAnswer]
(
	[ID]					INT					NOT NULL	PRIMARY KEY	IDENTITY(1, 1),
	[ParticipantID]			INT					NOT NULL,
	[QuestionID]			INT					NOT NULL,
	[Answer]				VARCHAR(200)		NOT NULL,
	CONSTRAINT FK_SubjectParticipantAnswer_Participant_ParticipantID FOREIGN KEY ([ParticipantID]) REFERENCES [Participant]([ID]),
	CONSTRAINT FK_SubjectParticipantAnswer_Question_QuestionID FOREIGN KEY ([QuestionID]) REFERENCES [Question]([ID])
);


INSERT INTO [User] ([FullName], [ExamCard], [Email], [Role], [Gender], [BirthDate], [Password], [IsActive]) VALUES ('Kesya Widya', 'Xidosp', 'kesyaaw@gmail.com', '1', '2', '2006-09-09', '123', 1);
INSERT INTO [User] ([FullName], [ExamCard], [Email], [Role], [Gender], [BirthDate], [Password], [IsActive])
VALUES 
	('John Doe', 'EX123456', 'johndoe@example.com', '2', '1', '1990-05-15', 'password123', 1),
	('Jane Smith', 'EX123457', 'janesmith@example.com', '2', '2', '1995-07-20', 'securepassword', 1),
	('Michael Johnson', 'EX123458', 'michaelj@example.com', '2', '1', '1988-02-10', 'michael123', 0),
	('Emily Davis', 'EX123459', 'emilydavis@example.com', '2', '2', '1993-11-30', 'emilypassword', 1),
	('David Brown', 'EX123460', 'davidbrown@example.com', '2', '1', '1985-09-25', 'davidssecurepass', 0),
	('Sarah Wilson', 'EX123461', 'sarahwilson@example.com', '2', '2', '1992-03-05', 'sarahw123', 1),
	('James Moore', 'EX123462', 'jamesmoore@example.com', '2', '1', '1980-06-15', 'jamespassword', 1),
	('Olivia Taylor', 'EX123463', 'oliviataylor@example.com', '2', '2', '1998-01-22', 'olivia12345', 1),
	('Benjamin White', 'EX123464', 'benjaminwhite@example.com', '2', '1', '1996-12-10', 'benjaminsecure', 1),
	('Sophia Harris', 'EX123465', 'sophiaharris@example.com', '2', '2', '1997-08-14', 'sophiahsecure', 1);


INSERT INTO [Subject] ([Name], [Time])
VALUES 
	('Mathematics', 120),
	('Physics', 30),
	('Chemistry', 60),
	('Biology', 150),
	('Computer Science', 180),
	('English Literature', 110);


-- Mathematics
INSERT INTO [Question] ([SubjectID], [Question], [OptionA], [OptionB], [OptionC], [OptionD], [CorrectAnswer])
VALUES
	(1, 'The area of ​​the shaded region is?,2025-01-31 213456.png', '125,74 cm2', '78,50 cm2', '28,26 cm2', '50,24 cm2', '50,24 cm2'),
	(1, 'What is the value of π to two decimal places?', '3.12', '3.14', '3.15', '3.16', '3.14'),
	(1, 'What is the integral of x^2?', 'x^3/3', 'x^3/2', '2x', 'x^2/2', 'x^3/3'),
	(1, 'Solve for x: 2x + 3 = 7', 'x = 2', 'x = 3', 'x = 4', 'x = 1', 'x = 2'),
	(1, 'What is the area of a circle with radius 4?', '16π', '8π', '12π', '4π', '16π'),
	(1, 'What is the square root of 144?', '12', '10', '14', '16', '12'),
	(1, 'What is the slope of the line y = 2x + 5?', '2', '5', '3', '1', '2'),
	(1, 'What is 25% of 200?', '50', '25', '100', '75', '50'),
	(1, 'What is the perimeter of a square with side length 6?', '24', '36', '12', '18', '24'),
	(1, 'If f(x) = 2x + 1, what is f(3)?', '7', '6', '5', '8', '7');


-- Physics
INSERT INTO [Question] ([SubjectID], [Question], [OptionA], [OptionB], [OptionC], [OptionD], [CorrectAnswer])
VALUES
	(2, 'What is the SI unit of force?', 'Newton', 'Joule', 'Watt', 'Pascal', 'Newton'),
	(2, 'What is the acceleration due to gravity on Earth?', '9.8 m/s^2', '10 m/s^2', '9.0 m/s^2', '8.9 m/s^2', '9.8 m/s^2'),
	(2, 'What is the formula for kinetic energy?', 'KE = 1/2 mv^2', 'KE = mv', 'KE = mgh', 'KE = 1/2 m^2v', 'KE = 1/2 mv^2'),
	(2, 'What is the speed of light in a vacuum?', '3 x 10^8 m/s', '3 x 10^6 m/s', '3 x 10^9 m/s', '3 x 10^7 m/s', '3 x 10^8 m/s'),
	(2, 'What does the law of conservation of energy state?', 'Energy cannot be created or destroyed', 'Energy is always increasing', 'Energy is always decreasing', 'Energy can be created but not destroyed', 'Energy cannot be created or destroyed'),
	(2, 'What is the formula for work done?', 'Work = Force x Distance', 'Work = Force x Speed', 'Work = Mass x Acceleration', 'Work = Force x Time', 'Work = Force x Distance'),
	(2, 'What is the main cause of the greenhouse effect?', 'Carbon dioxide', 'Oxygen', 'Nitrogen', 'Helium', 'Carbon dioxide'),
	(2, 'What type of wave is light?', 'Electromagnetic wave', 'Mechanical wave', 'Sound wave', 'Transverse wave', 'Electromagnetic wave'),
	(2, 'What is the unit of electric current?', 'Ampere', 'Volt', 'Ohm', 'Coulomb', 'Ampere'),
	(2, 'Which of the following is not a form of energy?', 'Water', 'Kinetic', 'Potential', 'Thermal', 'Water');


-- Chemistry
INSERT INTO [Question] ([SubjectID], [Question], [OptionA], [OptionB], [OptionC], [OptionD], [CorrectAnswer])
VALUES
	(3, 'What is the chemical formula for water?', 'H2O', 'O2', 'CO2', 'H2O2', 'H2O'),
	(3, 'Which element has the chemical symbol Na?', 'Sodium', 'Nitrogen', 'Neon', 'Magnesium', 'Sodium'),
	(3, 'What is the pH value of pure water?', '7', '4', '10', '3', '7'),
	(3, 'What is the chemical formula for methane?', 'CH4', 'CO2', 'C2H6', 'C3H8', 'CH4'),
	(3, 'Which of the following is a noble gas?', 'Argon', 'Oxygen', 'Nitrogen', 'Chlorine', 'Argon'),
	(3, 'What is the atomic number of carbon?', '6', '8', '12', '14', '6');


-- Biology
INSERT INTO [Question] ([SubjectID], [Question], [OptionA], [OptionB], [OptionC], [OptionD], [CorrectAnswer])
VALUES
	(4, 'What is the powerhouse of the cell?', 'Mitochondria', 'Nucleus', 'Ribosome', 'Endoplasmic Reticulum', 'Mitochondria'),
	(4, 'Which organ in the human body pumps blood?', 'Heart', 'Lungs', 'Liver', 'Kidneys', 'Heart'),
	(4, 'What is the basic unit of life?', 'Cell', 'Atom', 'Molecule', 'Organism', 'Cell'),
	(4, 'What process do plants use to make their own food?', 'Photosynthesis', 'Respiration', 'Digestion', 'Fermentation', 'Photosynthesis'),
	(4, 'Which part of the brain controls breathing and heart rate?', 'Medulla Oblongata', 'Cerebellum', 'Cerebrum', 'Thalamus', 'Medulla Oblongata'),
	(4, 'What is the genetic material found in the nucleus of a cell?', 'DNA', 'RNA', 'Proteins', 'Carbohydrates', 'DNA'),
	(4, 'Which of the following is an example of a unicellular organism?', 'Amoeba', 'Human', 'Dog', 'Tree', 'Amoeba'),
	(4, 'What type of blood cells are responsible for oxygen transport?', 'Red blood cells', 'White blood cells', 'Platelets', 'Plasma', 'Red blood cells'),
	(4, 'What is the process by which cells divide to form two identical daughter cells?', 'Mitosis', 'Meiosis', 'Fission', 'Fusion', 'Mitosis'),
	(4, 'What is the main component of the cell membrane?', 'Phospholipids', 'Proteins', 'Carbohydrates', 'Nucleic acids', 'Phospholipids');


-- Computer Science 
INSERT INTO [Question] ([SubjectID], [Question], [OptionA], [OptionB], [OptionC], [OptionD], [CorrectAnswer])
VALUES
	(5, 'What is the main function of the CPU in a computer?', 'Process data', 'Store data', 'Display data', 'Control peripherals', 'Process data'),
	(5, 'Which programming language is primarily used for web development?', 'JavaScript', 'Python', 'C++', 'Java', 'JavaScript'),
	(5, 'What does ''HTTP'' stand for?', 'Hypertext Transfer Protocol', 'Hyper Transfer Text Protocol', 'Home Transfer Text Protocol', 'High Transfer Protocol', 'Hypertext Transfer Protocol'),
	(5, 'Which of these is an example of an operating system?', 'Windows', 'Microsoft Word', 'Google Chrome', 'Photoshop', 'Windows'),
	(5, 'What does RAM stand for in computing?', 'Random Access Memory', 'Read Access Memory', 'Rapid Access Memory', 'Ready Access Memory', 'Random Access Memory'),
	(5, 'Which device is used to input data into a computer?', 'Keyboard', 'Monitor', 'Printer', 'Speaker', 'Keyboard'),
	(5, 'What does ''HTML'' stand for?', 'Hypertext Markup Language', 'Home Tool Markup Language', 'Hyper Transfer Markup Language', 'Hyper Text Management Language', 'Hypertext Markup Language'),
	(5, 'Which of the following is NOT a data structure?', 'Thread', 'Array', 'Stack', 'Queue', 'Thread'),
	(5, 'What does ''GUI'' stand for in computing?', 'Graphical User Interface', 'General User Interface', 'Graphical Universal Interface', 'General Universal Interface', 'Graphical User Interface'),
	(5, 'Which of the following is an example of a database management system?', 'MySQL', 'Notepad', 'Microsoft Word', 'Excel', 'MySQL');

-- English Language
INSERT INTO [Question] ([SubjectID], [Question], [OptionA], [OptionB], [OptionC], [OptionD], [CorrectAnswer])
VALUES
    (6, 'Which of the following is a synonym for ''happy''?', 'Joyful', 'Angry', 'Sad', 'Tired', 'Joyful'),
    (6, 'Guess the word? _A_,2025-02-01 213456.png', 'BAG', 'FAN', 'PAN', 'TAG', 'BAG'),
    (6, 'Which picture shows the table?', '2025-02-01 221030.png', '2025-02-01 221123.png', '2025-02-01 221234.png', '2025-02-01 231442.png', '2025-02-01 231442.png'),
    (6, 'Which number is shown in the picture?,2025-02-01 2423452.png', '23', '25', '21', '20', '25'),
    (6, 'Which of the following is an example of a compound sentence?', 'I like apples, but she likes oranges.', 'I like apples.', 'She likes oranges.', 'I like apples and oranges.', 'I like apples, but she likes oranges.'),
    (6, 'What is the past tense of the verb ''run''?', 'Ran', 'Run', 'Running', 'Runs', 'Ran'),
    (6, 'Which of these sentences is in the passive voice?', 'The book was read by Mary.', 'Mary read the book.', 'I am reading the book.', 'The book is reading by Mary.', 'The book was read by Mary.'),
    (6, 'Which is the correct plural form of ''child''?', 'Children', 'Childs', 'Childes', 'Childrens', 'Children'),
    (6, 'What part of speech is the word ''quickly''?', 'Adverb', 'Noun', 'Verb', 'Adjective', 'Adverb'),
    (6, 'Which of the following sentences is a declarative sentence?', 'She is coming to the party.', 'Is she coming to the party?', 'Come here now!', 'Please come here.', 'She is coming to the party.');



INSERT INTO [Participant] ([UserID], [SubjectID], [Date], [TimeTaken])
VALUES (2, 1, GETDATE(), 30);

INSERT INTO [ParticipantAnswer] ([ParticipantID], [QuestionID], [Answer])
VALUES
	(1, 1, '50,24 cm2'),
	(1, 2, '3.15'),  
	(1, 3, 'x^3/3'), 
	(1, 4, 'x = 3'), 
	(1, 5, '8π'),      
	(1, 6, '14'),      
	(1, 7, '5'),       
	(1, 8, '75'),      
	(1, 9, '24');    


INSERT INTO [Participant] ([UserID], [SubjectID], [Date], [TimeTaken])
VALUES (2, 1, '2025-02-01', 30);

INSERT INTO [ParticipantAnswer] ([ParticipantID], [QuestionID], [Answer])
VALUES
	(2, 1, '125,74 cm2'),  
	(2, 2, '3.12'),        
	(2, 3, 'x^3/3'),       
	(2, 4, 'x = 2'),       
	(2, 5, '16π'),         
	(2, 6, '12'),          
	(2, 7, '2'),           
	(2, 8, '50'),          
	(2, 9, '24'),         
	(2, 10, '7');         

	
INSERT INTO [Participant] ([UserID], [SubjectID], [Date], [TimeTaken])
VALUES (2, 6, GETDATE(), 45);

INSERT INTO [ParticipantAnswer] ([ParticipantID], [QuestionID], [Answer])
VALUES
	(3, 47, 'Joyful'), 
	(3, 48, 'BAG'),
	(3, 49, '2025-02-01 231442.png'),   
	(3, 50, '25'), 
	(3, 51, 'I like apples, but she likes oranges.'), 
	(3, 52, 'Ran'),    
	(3, 53, 'The book was read by Mary.'),
	(3, 54, 'Children'),
	(3, 55, 'Adverb'), 
	(3, 56, 'Come here now!');