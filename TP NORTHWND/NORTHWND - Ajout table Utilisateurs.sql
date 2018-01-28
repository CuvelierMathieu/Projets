USE NORTHWND;

CREATE TABLE Utilisateurs
(
	Id int identity (1, 1) PRIMARY KEY,
	Identifiant varchar(MAX),
	MotDePasse varchar(MAX),
);

GO