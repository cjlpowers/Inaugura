/***************************************************
OrbisTel Database
***************************************************/
DROP DATABASE IF EXISTS OrbisTel;
CREATE DATABASE IF NOT EXISTS OrbisTel;
USE OrbisTel;

/***************************************************
Switches
***************************************************/
DROP TABLE IF EXISTS Switches;
CREATE TABLE IF NOT EXISTS Switches
(	
	id varchar(40) NOT NULL,
	name varchar(100) NOT NULL,		
	xml text NOT NULL,
	PRIMARY KEY(id)	
);
DELETE FROM Switches;

/* Switch OutgoingCallReserve Table */
DROP TABLE IF EXISTS OutgoingCallReserve;
CREATE TABLE IF NOT EXISTS OutgoingCallReserve
(	
	switchId varchar(40) NOT NULL,
	startTime datetime NOT NULL,
	endTime datetime NOT NULL,
	count int NOT NULL,	
);
DELETE FROM OutgoingCallReserve;


/***************************************************
Media
***************************************************/
DROP TABLE IF EXISTS DTS;
CREATE TABLE IF NOT EXISTS DTS
(	
	id varchar(40) NOT NULL,
	name varchar(233) NOT NULL,	
	xml text NOT NULL,
	PRIMARY KEY(id)	
);
DELETE FROM DTS;

DROP TABLE IF EXISTS Files;
CREATE TABLE IF NOT EXISTS Files
(	
	fileName varchar(100) NOT NULL,	
	folder varchar(255) NOT NULL,
	version varchar(100) NOT NULL,	
	length int NOT NULL,
	data LONGBLOB NOT NULL,	
	PRIMARY KEY(fileName,folder,version)
);
DELETE FROM Files;

/***************************************************
Services
***************************************************/
DROP TABLE IF EXISTS Services;
CREATE TABLE IF NOT EXISTS Services
(	
	id varchar(40) NOT NULL,
	name varchar(233) NOT NULL,	
	xml text NOT NULL,
	PRIMARY KEY(id)	
);
DELETE FROM Services;