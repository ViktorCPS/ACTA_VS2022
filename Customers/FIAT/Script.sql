
--TACKA 1
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--DEFAULT VREDNOST za code IZ UI working_unit_id sa vodecim nulama npr. working_unit_id = 12 code = 0012
---------------------------------------------------------------------------------------------------------
ALTER TABLE working_units 
ADD code  VARCHAR(30) NOT NULL DEFAULT '0000';

--ALTER TABLE working_units
--   ADD CONSTRAINT UNIQ_working_units_code UNIQUE (code);
--Mozda ce dva utea imati isti code???

--TACKA 2
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Dodavanje novog grupisanja zaposlenih identicno kao za working_units
---------------------------------------------------------------------------------------------------------  
CREATE TABLE organizational_units 
(
	organizational_unit_id                INTEGER     NOT NULL PRIMARY KEY ,
	parent_organizational_unit_id         INTEGER     NOT NULL
		CONSTRAINT FK_organizational_units_organizational_units REFERENCES organizational_units (organizational_unit_id),
	description 	                        NVARCHAR(50)    NULL,
	name			                            NVARCHAR(50)    NULL,
	address_id 		                        INTEGER         NULL,
	status			                          VARCHAR(20)     NULL,
	code                                  VARCHAR(30) NOT NULL,
	created_by		                        VARCHAR(20)     NULL,
	created_time                          DATETIME        NULL,
	modified_by                           VARCHAR(20)     NULL,
	modified_time                         DATETIME        NULL
);
ALTER TABLE organizational_units 
	ADD CONSTRAINT DF_organizational_units_status DEFAULT ('ACTIVE') FOR status;

ALTER TABLE organizational_units
	ADD CONSTRAINT UNIQ_organizational_units_name UNIQUE (name);
INSERT INTO organizational_units
(organizational_unit_id,parent_organizational_unit_id,description,name,status,code)
VALUES
(0,0,'DEFAULT ORGANIZATIONAL UNIT','DEFAULT ORGANIZATIONAL UNIT','ACTIVE','0000');
--TACKA 2a
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Uspostavljenje veze izmedju working_units i organizational_units
---------------------------------------------------------------------------------------------------------  
CREATE TABLE working_units_x_organizational_units
(
	organizational_unit_id                INTEGER     NOT NULL,
	working_unit_id                       INTEGER     NOT NULL,
    purpose                               VARCHAR(20)     NULL, /*'REFERENCE','FILTER'*/
	created_by		              VARCHAR(20)     NULL,
	created_time                          DATETIME        NULL,
	modified_by                           VARCHAR(20)     NULL,
	modified_time                         DATETIME        NULL

);
ALTER TABLE working_units_x_organizational_units
   ADD CONSTRAINT PK_working_units_x_organizational_units PRIMARY KEY (organizational_unit_id,working_unit_id);

ALTER TABLE working_units_x_organizational_units
   ADD CONSTRAINT CHECK_working_units_x_organizational_units_purpose CHECK (purpose IN ('REFERENCE','FILTER'));

ALTER TABLE working_units_x_organizational_units
   ADD CONSTRAINT FK_working_units_x_organizational_units_organizational_units FOREIGN KEY (organizational_unit_id)
      REFERENCES organizational_units (organizational_unit_id);

ALTER TABLE working_units_x_organizational_units
   ADD CONSTRAINT FK_working_units_x_organizational_units_working_units FOREIGN KEY (working_unit_id)
      REFERENCES working_units (working_unit_id);
 
--TACKA 3
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Dodavanje zaposlenom kljuca organisation_units
---------------------------------------------------------------------------------------------------------  
ALTER TABLE employees 
	ADD organizational_unit_id             INTEGER     NOT NULL DEFAULT 0;
	
ALTER TABLE employees
	ADD CONSTRAINT FK_employees_organizational_units FOREIGN KEY (organizational_unit_id)
		REFERENCES organizational_units (organizational_unit_id);
		
		
--TACKA 8
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Dodavanje kategorije usera
---------------------------------------------------------------------------------------------------------  
CREATE TABLE appl_users_categories
(
	appl_users_category_id                INTEGER     NOT NULL,
	description 	                        NVARCHAR(50)    NULL,
	name			                            NVARCHAR(50)    NULL,
	created_by		                        VARCHAR(20)     NULL,
	created_time                          DATETIME        NULL,
	modified_by                           VARCHAR(20)     NULL,
	modified_time                         DATETIME        NULL
);

ALTER TABLE appl_users_categories
   ADD CONSTRAINT PK_appl_users_categories PRIMARY KEY (appl_users_category_id);
   
INSERT INTO appl_users_categories
(appl_users_category_id,description,name,created_by,created_time,modified_by,modified_time)
VALUES
(0,'DEFAULT CATEGORY','DEFAULT CATEGORY','SYS',GETDATE(),NULL,NULL);



CREATE TABLE appl_users_x_appl_users_categories
(
        appl_users_category_id                INTEGER     NOT NULL,
        user_id                               VARCHAR(20) NOT NULL,
        default_category                      INTEGER     NOT NULL,
	created_by		              VARCHAR(20)     NULL,
	created_time                          DATETIME        NULL,
	modified_by                           VARCHAR(20)     NULL,
	modified_time                         DATETIME        NULL
);


ALTER TABLE appl_users_x_appl_users_categories
   ADD CONSTRAINT PK_appl_users_x_appl_users_categories PRIMARY KEY (appl_users_category_id,user_id);

ALTER TABLE appl_users_x_appl_users_categories
   ADD CONSTRAINT CHECK_appl_users_x_appl_users_categories_default_category CHECK (default_category IN (0,1));

ALTER TABLE appl_users_x_appl_users_categories
   ADD CONSTRAINT FK_appl_users_x_appl_users_categories_x_appl_users FOREIGN KEY (user_id)
      REFERENCES appl_users (user_id);
      
ALTER TABLE appl_users_x_appl_users_categories
   ADD CONSTRAINT FK_appl_users_x_appl_users_categories_x_appl_users_categories FOREIGN KEY (appl_users_category_id)
      REFERENCES appl_users_categories (appl_users_category_id);


--TACKA 4
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Prava i privelgije po tipovima za kategorije usera
---------------------------------------------------------------------------------------------------------
CREATE TABLE appl_users_categories_x_pass_types
(
	appl_users_category_id                INTEGER     NOT NULL,
	pass_type_id		                      INTEGER 		NOT NULL,
	purpose                               VARCHAR(20)	NOT NULL,
	created_by		                        VARCHAR(20)     NULL,
	created_time                          DATETIME        NULL,
	modified_by                           VARCHAR(20)     NULL,
	modified_time                         DATETIME        NULL
);
ALTER TABLE appl_users_categories_x_pass_types
   ADD CONSTRAINT PK_appl_users_categories_x_pass_types PRIMARY KEY (appl_users_category_id,pass_type_id,purpose);

--FK
ALTER TABLE appl_users_categories_x_pass_types
   ADD CONSTRAINT FK_appl_users_categories_x_pass_types_appl_users_categories FOREIGN KEY (appl_users_category_id)
       REFERENCES appl_users_categories (appl_users_category_id);

ALTER TABLE appl_users_categories_x_pass_types
   ADD CONSTRAINT FK_appl_users_categories_x_pass_types_pass_types FOREIGN KEY (pass_type_id)
       REFERENCES pass_types (pass_type_id);

--TACKA 5 
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Prosirivanje pass_types tabele limitima i kompanijama; kompanija je WU koji je sam sebi rodtelj

CREATE TABLE pass_type_limits
(
	pass_type_limit_id                    INTEGER 	   NOT  NULL,
        name                                  NVARCHAR(50) NOT  NULL,
	limit_type                            VARCHAR(20)  NOT  NULL,
	limit_value                           INTEGER      NOT  NULL,
	limit_measure_unit                    VARCHAR(20)  NOT  NULL,
	limit_period                          INTEGER      NOT  NULL,
	limit_start_date                      DATETIME     NOT  NULL,
	created_by		              VARCHAR(20)       NULL,
	created_time                          DATETIME          NULL,
	modified_by                           VARCHAR(20)       NULL,
	modified_time                         DATETIME          NULL
);

ALTER TABLE pass_type_limits
  ADD CONSTRAINT PK_pass_type_limits PRIMARY KEY (pass_type_limit_id);
  
ALTER TABLE pass_type_limits
   ADD CONSTRAINT CHK_pass_type_limits_limit_type CHECK (limit_type IN ('ELEMENTARY','COMPOSITE','OCCASSIONALY'));
   
ALTER TABLE pass_type_limits
   ADD CONSTRAINT CHK_pass_type_limits_limit_measure_unit CHECK (limit_measure_unit IN ('DAYS'));

ALTER TABLE actamgr.pass_type_limits
 ADD CONSTRAINT UNIQ_pass_type_limits_name UNIQUE(name);

ALTER TABLE pass_types
   ADD description_alternative         NVARCHAR(50)          NOT NULL DEFAULT 'N/A';

ALTER TABLE pass_types
ALTER COLUMN description NVARCHAR(50) NOT NULL;


ALTER TABLE pass_types
   ADD segment_color                   VARCHAR(20)           NULL;

ALTER TABLE pass_types
   ADD limit_composite_id              INTEGER 		     NULL;

ALTER TABLE pass_types
   ADD limit_elementary_id             INTEGER 		     NULL;

ALTER TABLE pass_types
   ADD limit_occasion_id               INTEGER 		     NULL;   
   
ALTER TABLE pass_types
   ADD working_unit_id                 INTEGER     NULL DEFAULT 0;
   
ALTER TABLE pass_types
   ADD CONSTRAINT FK_pass_types_pass_type_limits_limit_composite_id  FOREIGN KEY (limit_composite_id )
      REFERENCES pass_type_limits (pass_type_limit_id );

ALTER TABLE pass_types
   ADD CONSTRAINT FK_pass_types_pass_type_limits_limit_elementary_id  FOREIGN KEY (limit_elementary_id )
      REFERENCES pass_type_limits (pass_type_limit_id );

ALTER TABLE pass_types
   ADD CONSTRAINT FK_pass_types_pass_type_limits_limit_occasion_id  FOREIGN KEY (limit_occasion_id )
      REFERENCES pass_type_limits (pass_type_limit_id );


ALTER TABLE pass_types
   ADD CONSTRAINT FK_pass_types_working_unit_id FOREIGN KEY (working_unit_id)
      REFERENCES working_units (working_unit_id);
 

ALTER TABLE pass_types
   ADD confirmation_flag                INTEGER 	NOT  NULL DEFAULT 0;    

ALTER TABLE pass_types
   ADD massive_input                    INTEGER 	NOT  NULL DEFAULT 0; 

ALTER TABLE pass_types
   ADD manual_input_flag                INTEGER 	NOT  NULL DEFAULT 0;

ALTER TABLE pass_types
   ADD verification_flag                INTEGER 	NOT  NULL DEFAULT 0;   
    

--ALTER TABLE pass_types 
--	ADD CONSTRAINT DF_pass_types_confirmation_flag DEFAULT (0) FOR confirmation_flag;
	
ALTER TABLE pass_types
   ADD confirmation_pass_type_id        INTEGER 	NULL;
   

--FK???
--NO because it is SELF CONSTRAINT
---------------------------------------------------------------------------------------------------------  

--TACKA 6
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Dodavanje tabele sa pravilima
---------------------------------------------------------------------------------------------------------  
CREATE TABLE rules
(
  rule_id                               INTEGER      NOT NULL,
  working_unit_id                       INTEGER      NOT NULL,
  rule_type                             VARCHAR(200)  NOT NULL,
  employee_type_id                      INTEGER      NOT NULL,
  rule_description                      NVARCHAR(1000)   NULL,
  rule_value                            INTEGER      NOT NULL,
  rule_datetime_1                        DATETIME         NULL,
  rule_datetime_2                        DATETIME         NULL,
	created_by		                        VARCHAR(20)      NULL,
	created_time                          DATETIME         NULL,
	modified_by                           VARCHAR(20)      NULL,
	modified_time                         DATETIME         NULL
);
ALTER TABLE rules
   ADD CONSTRAINT PK_rules PRIMARY KEY (rule_id);

INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(1,1,'DELAY RULE MAX MIN',1,'Maksimalno vreme od pocetka RV koje se tretira kao Kasnjenje',60,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(2,1,'DELAY RULE ROUNDING',1,'Broj minuta na koji se zakasnjenje racuna',15,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(3,1,'PRESENCE MIN',1,'Minimalan broj minuta koji se racuna kao prisustvo',15,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(4,1,'PRESENCE/ABSENCE ROUNDING RULE',1,'Minimalno vreme da se racuna prisustvo/dopuna za odsustvo',5,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(5,1,'OVERTIME ROUNDING RULE',1,'Zaokruzivanje prekovremenog',15,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(6,1,'OVERTIME MINIMUM ',1,'Minimalno vreme za rad van radnog vremena',30,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(7,1,'OVERTIME SHIFT START',1,'Vreme u minutama pre RV koje se ne tretira kao rad van radnog vremena',60,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(8,1,'OVERTIME SHIFT END',1,'Vreme u minutama posle RV koje se ne tretira kao rad van radnog vremena',30,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(9,1,'OVERTIME OUT OF WS MINIMUM',1,' Minimalno vreme u minutama u danima kada ne treba da radi koje se tretira kao rad van radnog vremena',30,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(10,1,'OVERTIME ROUNDING RULE OUT OF WS',1,'Zaokruzivanje prekovremenog u danima kada ne treba da radi',30,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(11,1,'NIGHT WORK',1,'Vreme kada se racuna nocni rad',0,'1900-01-01 22:00:00','1900-01-01 06:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('22','1','BANK HOURS DAY LIMIT','1','Dnevni limit broja sati koji mogu da idu u zara?ene sate','4','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('23','1','BANK HOURS WEEK LIMIT','1','Nedeljni limit broja sati koji mogu da idu u zara?ene sate','8','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('24','1','OVERTIME HOURS DAY LIMIT','1','Dnevni limit broja sati koji mogu biti proglašeni za prekovremeno','4','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('25','1','OVERTIME HOURS WEEK LIMIT','1','Nedeljni limit broja sati koji mogu biti proglašeni za prekovremeno','8','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('26','1','COMPANY ANNUAL LEAVE','1','ID tipa prolaska za godišnji odmor','5','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('13','1','COMPANY REGULAR WORK','1','ID tipa za redovan rad zaposlenih u kompaniji','-3001','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('12','1','CUTOFF DATE','1','Dan u mesecu do kog je mogu?e menjati podatke u procesiranim parovima','15','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('16','1','COMPANY DELAY','1','ID tipa prolaska za kasnjenje','-2001','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('19','1','COMPANY INITIAL OVERTIME','1','ID tipa prolaska za prekovremeno','-1000','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('27','1','COMPANY OVERTIME PAID','1','ID tipa za prekovremeno pla?eno','-1003','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('28','1','COMPANY BANK HOUR','1','ID tipa za zara?ene sate','-1002','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('29','1','COMPANY BANK HOUR USED','1','ID tipa za koriscenje zaradjenih sati','23','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('30','1','COMPANY STOP WORKING','1','ID tipa za prestanak rada','20','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('31','1','COMPANY STOP WORKING DONE','1','ID tipa za odradu prestanka rada','24','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('40','1','EMPLOYEE LOANS MAXIMUM PERIOD','1','Maksimalna duzina perioda za pozajmicu radnika','14','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('43', '100000','PERSONAL HOLIDAY PASS TYPE','21','Licni verski praznik zaposlenog','-10','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('44', '100000','PERSONAL HOLIDAY PASS TYPE','22','Licni verski praznik zaposlenog','-10','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('45', '100000','PERSONAL HOLIDAY PASS TYPE','23','Licni verski praznik zaposlenog','-10','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('46', '100000','PERSONAL HOLIDAY PASS TYPE','24','Licni verski praznik zaposlenog','-10','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('47', '100000','COMPANY OVERTIME REJECTED','24','Odbijeni prekovremeni sati','-1001','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('47', '100000','COMPANY OFFICIAL TRIP','1','Sluzbeni put','-1001','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('47', '100000','PERSONAL HOLIDAY PASS TYPE','25','Licni verski praznik zaposlenog','-10','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('45', '1','HRSSC CUTOFF DATE','1','Dan u mesecu do kog je mogu?e menjati podatke u procesiranim parovima za HRSSC','10','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('46', '1','BANK HOURS USED ROUNDING RULE','1','Pravilo zaokruzivanja za koriscenje zara?enih sati','30','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('47', '1','COMPANY SICK LEAVE NCF','1','Nepotvrdjeno bolovanje','25','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('48', '1','WCDR CUT OFF DATE','1','Dan u mesecu do kog je moguce menjati podatke u procesiranim parovima za WCDR','4','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('48', '1','BANK HOURS CUT OFF DATE 1','1','Dan kada se isplacuju zaradjeni sati','0','1900-06-30 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('48', '1','BANK HOURS CUT OFF DATE 2','1','Dan kada se isplacuju zaradjeni sati','0','1900-12-31 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('3391', '-5','OFFICIAL TRIP DURATION','1','Vreme trajanja sluzbenog puta u satima','8','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('396', '-2','BANK HOURS LIMIT','1','Limit za koriscenje zaradjenih sati','8','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('416', '-2','SICK LEAVE REFUNDATION TYPE 1','1','Bolovanje koje refundira drzava','27','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('10038', '0','EXPATRIAT OUT TYPE','1','Tip expatriat out zaposlenih','8','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('446', '-2', 'COMPANY PAID LEAVE','1','Placeno odsustvo usled nedostatka posla','22','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
('3451', '-5','COMPANY PAID LEAVE 65 PERCENT','1','Placeno odsustvo usled nedostatka posla (65%)','3058','1900-01-01 00:00:00','1900-01-01 00:00:00','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(3461,-5,'COMPANY INITIAL OVERTIME USED',1,'Tip koriscenja sati iz buffera za neopredeljene prekovremene sate',3087,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(470,-2,'INITIAL OVERTIME USED ROUNDING',5,'Pravilo zaokruzivanja za koriscenje neopredeljenih prekovremenih sati',1,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(471,-2,'MC SCHEDULING BORDER DAY',1,'Dan od kog se zakazuju pregledi za naredni mesec',22,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(476,-2,'COMPANY INITIAL NIGHT OVERTIME',1,'Tip za prekovremeni rad nocu - za parove koji se automatski zatvaraju',88,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(481,-2,'COMPANY COLLECTIVE ANNUAL LEAVE',1,'Tip za kolektivni godisnji odmor',89,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(486,-2,'COMPANY COLLECTIVE ANNUAL LEAVE RESERVATION',1,'Broj dana rezervisan za kolektivni godisnji odmor',19,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(491,-2,'LABOR LAW',1,'Da li se validira interval izmedju zadatih smena i vreme proteklo izmenju intervala rada',1,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(496,-2,'STRIKE',1,'ID tipa prolaska za strajk',65,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(501,-2,'JUSTIFIED ABSENCE',1,'ID tipa prolaska za opravdano odsustvo',13,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(506,-2,'BANK HOURS BEFORE VACATION',1,'Da li se pre koriscenja godisnjeg odmora moraju najpre iskoristiti zaradjeni sati',1,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(511,-2,'OVERTIME UNJUSTIFIED HOURS BEFORE VACATION',1,'Da li se pre koriscenja godisnjeg odmora moraju najpre iskoristiti neopredeljeni prekovremeni sati',1,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(516,-2,'OVERTIME MONTH LIMIT',1,'Mesecni limit za isplatu prekovremenih sati',36,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(3521,-5,'INITIAL ABSENCE',1,'Tip inicijelnog odsustva',3001,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(10039,0,'SYSTEM PAIRS BY LOCATION',1,'Da li je omogucen prikaz i po lokacijama',1,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(521,-2,'NOTJUSTIFIED ABSENCE',1,'ID tipa prolaska za neopravdano odsustvo',14,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(10040,0,'SYSTEM HIDE SUPERVISORS BANK HOURS',1,'Da li je prikaz banke sati supervisora skriven od svih osim HRSSC operatera',1,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(518,-2,'WHOLE DAY ABSENCE BY HOUR 1',7,'Celodnevni tipovi koji HRSSC operateri mogu zadavati na sat',73,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(3526,-5,'MAX ABSENCE BETWEEN LOCATIONS',1,'Maksimalno odsustvo u minutima pri prelaska sa lokacje na lokaciju',60,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO ACTA_fiat.actamgr.rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(20463,0,'SYSTEM WRITTING DATA TO TAG',1,'Da li se upisuju podaci u karticu',1,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(536,-2,'NEGATIVE BANK HOURS',1,'ID tipa prolaska za negativnu banku sati za isplatu',92,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(541,-2,'STOP WORKING PAYMENT',1,'ID tipa prolaska za sate prekida rada za isplatu',93,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(546,-2,'COMPANY SICK LEAVE 30 DAYS CONTINUATION',1,'ID tipa prolaska za bolovanje do 30 dana nastavak',39,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(551,-2,'COMPANY SICK LEAVE INDUSTRIAL INJURY',1,'ID tipa prolaska za bolovanje povreda na radu',26,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(556,-2,'COMPANY SICK LEAVE INDUSTRIAL INJURY CONTINUATION',1,'ID tipa prolaska za bolovanje povreda na radu nastavak',40,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);
INSERT INTO rules
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time,modified_by,modified_time)
VALUES
(486,-2,'COMPANY ANNUAL LEAVE RESERVATION',1,'Broj rezervisanih dana godisnjeg odmora',6,'1900-01-01','1900-01-01','SYS',GETDATE(),NULL,NULL);


/*
MEAL RULE - MINIMAL PRESENCE
TRANSPORT ALLOWANCE - MINIMAL PRESENCE
CLOCK IN/OUT RULE
ANNUAL LEAVE CUTOFF DATE
*/

ALTER TABLE rules
  ADD CONSTRAINT FK_rules_working_units FOREIGN KEY (working_unit_id)
    REFERENCES working_units (working_unit_id);





CREATE TABLE employee_types
(
	employee_type_id                      INTEGER      NOT NULL,
	working_unit_id                       INTEGER      NOT NULL,
	employee_type_name                    NVARCHAR(30) NOT NULL,
	created_by		                        VARCHAR(20)      NULL,
	created_time                          DATETIME         NULL,
	modified_by                           VARCHAR(20)      NULL,
	modified_time                         DATETIME         NULL
);

--PK?????????????????????????????????//
 
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(1,-2,'BC','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(2,-2,'WC','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(3,-2,'Professional','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(4,-2,'Professional Expert','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(5,-2,'Manager','SYS',GETDATE(),NULL,NULL);

INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(6,-3,'BC','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(7,-3,'WC','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(8,-3,'Professional','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(9,-3,'Professional Expert','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(10,-3,'Manager','SYS',GETDATE(),NULL,NULL);

INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(11,-4,'BC','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(12,-4,'WC','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(13,-4,'Professional','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(14,-4,'Professional Expert','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(15,-4,'Manager','SYS',GETDATE(),NULL,NULL);

INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(16,-5,'BC','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(17,-5,'WC','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(18,-5,'Professional','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(19,-5,'Professional Expert','SYS',GETDATE(),NULL,NULL);
INSERT INTO employee_types
(employee_type_id,working_unit_id,employee_type_name,created_by,created_time,modified_by,modified_time)
VALUES
(20,-5,'Manager','SYS',GETDATE(),NULL,NULL);


ALTER TABLE employees
ADD employee_type_id                      INTEGER      NOT NULL;

--ALTER table employees
--   ADD CONSTRAINT FK_employees_employee_types FOREIGN KEY (employee_type_id)
--     REFERENCES employee_types (employee_type_id);

--TACKA 7
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Dodatna tabela parova na koje su primenjena pravila
---------------------------------------------------------------------------------------------------------  

CREATE TABLE io_pairs_processed
(
  rec_id                                BIGINT IDENTITY (1,1) NOT NULL,
	io_pair_id                            BIGINT           NULL,
	io_pair_date                          DATETIME     NOT NULL,
	employee_id                           INTEGER      NOT NULL,
	location_id                           INTEGER      NOT NULL,
	is_wrk_hrs_counter                    INTEGER      NOT NULL,
	pass_type_id                          INTEGER      NOT NULL,
	start_time                            DATETIME     NOT NULL,
	end_time                              DATETIME     NOT NULL,
	manual_created                        INTEGER      NOT NULL,
	confirmation_flag                     INTEGER 	   NOT NULL,
        confirmed_by		              VARCHAR(20)      NULL,
        confirmation_time                     DATETIME         NULL,
        verification_flag                     INTEGER 	   NOT NULL,
        verified_by		              VARCHAR(20)      NULL,
        verification_time                     DATETIME         NULL,
	alert                                 VARCHAR(200)     NULL,
        description                           NVARCHAR(500)    NULL,
	created_by		              VARCHAR(20)      NULL,
	created_time                          DATETIME         NULL,
	modified_by                           VARCHAR(20)      NULL,
	modified_time                         DATETIME         NULL
);

ALTER TABLE io_pairs_processed 
	ADD CONSTRAINT PK_io_pairs_processed PRIMARY KEY (rec_id);

ALTER TABLE io_pairs_processed
	ADD CONSTRAINT UNIQ_io_pairs_processed UNIQUE (io_pair_date,employee_id,start_time,end_time);

ALTER TABLE io_pairs
ADD pair_processed_gen_used INTEGER NOT NULL DEFAULT 0;


ALTER TABLE io_pairs_processed
  ADD CONSTRAINT FK_io_pairs_processed_employees FOREIGN KEY  (employee_id)
     REFERENCES employees (employee_id);

ALTER TABLE io_pairs_processed
  ADD CONSTRAINT FK_io_pairs_processed_locations FOREIGN KEY (location_id)
     REFERENCES locations (location_id);

ALTER TABLE io_pairs_processed
  ADD CONSTRAINT FK_io_pairs_processed_pass_types FOREIGN KEY (pass_type_id)
     REFERENCES pass_types (pass_type_id);

CREATE TABLE io_pairs_processed_hist
(
  rec_id                                BIGINT IDENTITY (1,1) NOT NULL,
	io_pair_id                            BIGINT           NULL,
	io_pair_date                          DATETIME     NOT NULL,
	employee_id                           INTEGER      NOT NULL,
	location_id                           INTEGER      NOT NULL,
	is_wrk_hrs_counter                    INTEGER      NOT NULL,
	pass_type_id                          INTEGER      NOT NULL,
	start_time                            DATETIME     NOT NULL,
	end_time                              DATETIME     NOT NULL,
	manual_created                        INTEGER      NOT NULL,
	confirmation_flag                     INTEGER 	   NOT NULL,
        confirmed_by		              VARCHAR(20)      NULL,
        confirmation_time                     DATETIME         NULL,
        verification_flag                     INTEGER 	   NOT NULL,
        verified_by		              VARCHAR(20)      NULL,
        verification_time                     DATETIME         NULL,
	alert                                 VARCHAR(200)     NULL,
        description                           NVARCHAR(500)    NULL,
	created_by		              VARCHAR(20)      NULL,
	created_time                          DATETIME         NULL,
	modified_by                           VARCHAR(20)      NULL,
	modified_time                         DATETIME         NULL
);

ALTER TABLE io_pairs_processed_hist
	ADD CONSTRAINT PK_io_pairs_processed_hist PRIMARY KEY (rec_id);


--TACKA 9
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--prosirena tabela holidays; razmotriti da li da se zove holidays_extended zbog kompatibilnosti na dole
---------------------------------------------------------------------------------------------------------
CREATE TABLE holidays_extended
(
        rec_id                            INTEGER       IDENTITY(1,1) NOT NULL,
	description                           NVARCHAR(128) NOT NULL,
	type                                  VARCHAR(20)   NOT NULL,
	category                              VARCHAR(20)   NOT NULL,
	year                                  DATETIME      NOT NULL,
	date_start                            DATETIME      NOT NULL,
	date_end                              DATETIME      NOT NULL,
	created_by		                      VARCHAR(20)      NULL,
	created_time                          DATETIME         NULL,
	modified_by                           VARCHAR(20)      NULL,
	modified_time                         DATETIME         NULL
);

ALTER TABLE holidays_extended
   ADD CONSTRAINT PK_holidays_extended PRIMARY KEY (rec_id);

ALTER TABLE holidays_extended
   ADD CONSTRAINT UNIQ_holidays_extended UNIQUE  (date_start,date_end,category);

ALTER TABLE holidays_extended
   ADD CONSTRAINT CHK_holidays_extended_type CHECK (type IN ('NATIONAL','PERSONAL'));
   
ALTER TABLE holidays_extended
   ADD CONSTRAINT CHK_holidays_extended_category CHECK (category IN (NULL,'I','II','III','N/A'));


--TACKA 12
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Tabela u kojoj se vode pozajmice zaposlenog (loans)
---------------------------------------------------------------------------------------------------------  
CREATE TABLE employee_loans
(
	rec_id                                BIGINT IDENTITY (1,1) NOT NULL,
	employee_id                           INTEGER      NOT NULL,
	date_start                            DATETIME         NULL,
	date_end                              DATETIME         NULL,
	working_unit_id                       INTEGER      NOT NULL,
	created_by		              VARCHAR(20)      NULL,
	created_time                          DATETIME         NULL,
	modified_by                           VARCHAR(20)      NULL,
	modified_time                         DATETIME         NULL
);

ALTER TABLE employee_loans
   ADD CONSTRAINT PK_employee_loans PRIMARY KEY (rec_id);

   
ALTER TABLE employee_loans
  ADD CONSTRAINT FK_employee_loans_employees FOREIGN KEY (employee_id)
     REFERENCES employees (employee_id);
          
     ALTER TABLE employee_loans
  ADD CONSTRAINT FK_employee_loans_working_units FOREIGN KEY(working_unit_id)
     REFERENCES working_units (working_unit_id);


--TACKA 14
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Counters za zaposlene
---------------------------------------------------------------------------------------------------------  
CREATE TABLE employee_counter_types
(

	employee_counter_type_id              INTEGER      NOT NULL,
        name			              NVARCHAR(50) NOT NULL,
        name_alternative	              NVARCHAR(50) NOT NULL,
	description 	                      NVARCHAR(50)     NULL,
	created_by		              VARCHAR(20)      NULL,
	created_time                          DATETIME         NULL,
	modified_by                           VARCHAR(20)      NULL,
	modified_time                         DATETIME         NULL
);

ALTER TABLE employee_counter_types
   ADD CONSTRAINT PK_employee_counter_type PRIMARY KEY (employee_counter_type_id);

ALTER TABLE employee_counter_types
   ADD CONSTRAINT UNIQ_employee_counter_types_name UNIQUE (name);

CREATE TABLE employee_counter_values
(

	employee_counter_type_id              INTEGER      NOT NULL,
        employee_id                           INTEGER      NOT NULL,
        value                                 INTEGER      NOT NULL,
        measure_unit                          VARCHAR(20)  NOT NULL,
	created_by		              VARCHAR(20)      NULL,
	created_time                          DATETIME         NULL,
	modified_by                           VARCHAR(20)      NULL,
	modified_time                         DATETIME         NULL
);

ALTER TABLE employee_counter_values
   ADD CONSTRAINT PK_employee_counter_values PRIMARY KEY (employee_counter_type_id,employee_id);

 ALTER TABLE employee_counter_values
   ADD CONSTRAINT FK_employee_counter_values_employee_counter_types FOREIGN KEY(employee_counter_type_id)
       REFERENCES employee_counter_types (employee_counter_type_id);

ALTER TABLE employee_counter_values
   ADD CONSTRAINT FK_employee_counter_values_employees FOREIGN KEY (employee_id)
       REFERENCES employees (employee_id);

CREATE TABLE employee_counter_values_hist
(
        rec_id                                BIGINT       NOT NULL IDENTITY(1,1),
	employee_counter_type_id              INTEGER      NOT NULL,
        employee_id                           INTEGER      NOT NULL,
        value                                 INTEGER      NOT NULL,
        measure_unit                          VARCHAR(20)  NOT NULL,
	created_by		              VARCHAR(20)      NULL,
	created_time                          DATETIME         NULL,
	modified_by                           VARCHAR(20)      NULL,
	modified_time                         DATETIME         NULL
);

ALTER TABLE employee_counter_values_hist
   ADD CONSTRAINT PK_employee_counter_values_hist PRIMARY KEY (rec_id);

ALTER TABLE employee_counter_values_hist
ADD description NVARCHAR(50) NULL;


--TACKA 15
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Prosirivanje tabele employees_asco4 sa po pet dodatnih polja
---------------------------------------------------------------------------------------------------------  
ALTER TABLE employees_asco4
ADD integer_value_6          INTEGER    NULL;

ALTER TABLE employees_asco4
ADD integer_value_7          INTEGER    NULL;

ALTER TABLE employees_asco4
ADD integer_value_8          INTEGER    NULL;

ALTER TABLE employees_asco4
ADD integer_value_9          INTEGER    NULL;

ALTER TABLE employees_asco4
ADD integer_value_10         INTEGER    NULL;

ALTER TABLE employees_asco4
ADD datetime_value_6         DATETIME   NULL;

ALTER TABLE employees_asco4
ADD datetime_value_7         DATETIME   NULL;

ALTER TABLE employees_asco4
ADD datetime_value_8         DATETIME   NULL;

ALTER TABLE employees_asco4
ADD datetime_value_9         DATETIME   NULL;

ALTER TABLE employees_asco4
ADD datetime_value_10        DATETIME   NULL;

ALTER TABLE employees_asco4
ADD nvarchar_value_6         NVARCHAR(256) NULL;

ALTER TABLE employees_asco4
ADD nvarchar_value_7         NVARCHAR(256)   NULL;

ALTER TABLE employees_asco4
ADD nvarchar_value_8         NVARCHAR(256)   NULL;

ALTER TABLE employees_asco4
ADD nvarchar_value_9         NVARCHAR(256)   NULL;

ALTER TABLE employees_asco4
ADD nvarchar_value_10        NVARCHAR(256)   NULL;

--TACKA 16
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Prosirivanje vradnosi tabele employees_asco4_metadata sa po pet dodatnih polja
---------------------------------------------------------------------------------------------------------  
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('integer_value_6',NULL);
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('integer_value_7',NULL);

INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('integer_value_8',NULL);

INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('integer_value_9',NULL);

INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('integer_value_10',NULL);
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('datetime_value_6',NULL);
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('datetime_value_7',NULL);
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('datetime_value_8',NULL);
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('datetime_value_9',NULL);
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('datetime_value_10',NULL);
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('nvarchar_value_6',NULL);
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('nvarchar_value_7',NULL);
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('nvarchar_value_8',NULL);
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('nvarchar_value_9',NULL);
INSERT INTO employees_asco4_metadata
(col,name)
VALUES
('nvarchar_value_10',NULL);

--TACKA
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--X ref tabela za appl users i organizational units
---------------------------------------------------------------------------------------------------------

CREATE TABLE appl_users_x_organizational_units
(
user_id 		              VARCHAR(20) NOT NULL	,
organizational_unit_id                INTEGER     NOT NULL      , 
purpose			             VARCHAR(20)	NOT NULL
);

ALTER TABLE appl_users_x_organizational_units
	ADD CONSTRAINT PK_appl_users_x_organizational_units PRIMARY KEY (user_id,organizational_unit_id,purpose);
	
ALTER TABLE appl_users_x_organizational_units
	ADD CONSTRAINT FK_appl_users_x_organizational_units_appl_users FOREIGN KEY (user_id)
		REFERENCES appl_users (user_id);
		
ALTER TABLE appl_users_x_organizational_units
	ADD CONSTRAINT FK_appl_users_x_organizational_units_organizational_units FOREIGN KEY (organizational_unit_id)
		REFERENCES organizational_units (organizational_unit_id);
	           

--TACKA 
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
-- new table for confirming pass types (one to many)
---------------------------------------------------------------------------------------------------------  
CREATE TABLE pass_types_confirmation
(
pass_type_id		             INTEGER      NOT NULL,
confirmation_pass_type_id        INTEGER 	  NOT NULL,
created_by		                 VARCHAR(20)      NULL,
created_time                     DATETIME         NULL,
modified_by                      VARCHAR(20)      NULL,
modified_time                    DATETIME         NULL
);

ALTER TABLE pass_types_confirmation
ADD CONSTRAINT PK_pass_types_confirmation PRIMARY KEY (pass_type_id,confirmation_pass_type_id);

ALTER TABLE pass_types_confirmation
ADD CONSTRAINT FK_pass_types_confirmation_pass_types_pass_type_id FOREIGN KEY (pass_type_id)
  REFERENCES pass_types (pass_type_id);

ALTER TABLE pass_types_confirmation
ADD CONSTRAINT FK_pass_types_confirmation_pass_types_confirmation_pass_type_id FOREIGN KEY (confirmation_pass_type_id)
  REFERENCES pass_types (pass_type_id);

ALTER TABLE pass_types
   DROP confirmation_pass_type_id;

--TACKA
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Prosirivanje tabele holidays_extended sa flagom da li se praznik ako pada u nedelju prenosi ili ne
---------------------------------------------------------------------------------------------------------  
ALTER TABLE holidays_extended
      ADD  sunday_transferable                   INTEGER       NULL;
--TACKA
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Prosirenje tabele sa satnicama hdr sa dva atributa: company i status
---------------------------------------------------------------------------------------------------------  
ALTER TABLE time_schema_hdr
   ADD status VARCHAR(20)  NULL;

ALTER TABLE time_schema_hdr
ADD CONSTRAINT CHK_time_schema_hdr_status CHECK (status IN ('ACTIVE','RETIRED'));

ALTER TABLE time_schema_hdr
   ADD working_unit_id INTEGER NULL;
--TACKA
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Prosirenje tabele sa satnicama dtl sa atributom: description
---------------------------------------------------------------------------------------------------------

ALTER TABLE time_schema_dtl
   ADD description VARCHAR(5) NULL;


CREATE TABLE employees_hist
(
    rec_id                    BIGINT IDENTITY(1,1) NOT NULL,     
	employee_id               int          NOT NULL,
	first_name                nvarchar(50) NOT NULL,
	last_name                 nvarchar(50) NOT NULL,
	working_unit_id           int NOT NULL,
	status                    varchar(20) NOT NULL,
	password                  varchar(10) NULL,
	address_id                int NULL,
	employee_group_id         int NULL,
	picture                   varchar(256) NULL,
	created_by                varchar(20) NULL,
	created_time              datetime NULL,
	modified_by               varchar(20) NULL,
	modified_time             datetime NULL,
	type                      varchar(20) NOT NULL,
	access_group_id           int NOT NULL,
	organizational_unit_id    int NOT NULL,
	employee_type_id          int NOT NULL
);

ALTER TABLE employees_hist
   ADD CONSTRAINT PK_employees_hist PRIMARY KEY (rec_id);



CREATE TABLE employees_asco4_hist
(
    rec_id  BIGINT IDENTITY(1,1) NOT NULL,  
	employee_id int NOT NULL,
	integer_value_1 int NULL,
	integer_value_2 int NULL,
	integer_value_3 int NULL,
	integer_value_4 int NULL,
	integer_value_5 int NULL,
	integer_value_6 int NULL,
	integer_value_7 int NULL,
	integer_value_8 int NULL,
	integer_value_9 int NULL,
	integer_value_10 int NULL,
	datetime_value_1 datetime NULL,
	datetime_value_2 datetime NULL,
	datetime_value_3 datetime NULL,
	datetime_value_4 datetime NULL,
	datetime_value_5 datetime NULL,
	datetime_value_6 datetime NULL,
	datetime_value_7 datetime NULL,
	datetime_value_8 datetime NULL,
	datetime_value_9 datetime NULL,
	datetime_value_10 datetime NULL,
	nvarchar_value_1 nvarchar(256) NULL,
	nvarchar_value_2 nvarchar(256) NULL,
	nvarchar_value_3 nvarchar(256) NULL,
	nvarchar_value_4 nvarchar(256) NULL,
	nvarchar_value_5 nvarchar(256) NULL,
	nvarchar_value_6 nvarchar(256) NULL,
	nvarchar_value_7 nvarchar(256) NULL,
	nvarchar_value_8 nvarchar(256) NULL,
	nvarchar_value_9 nvarchar(256) NULL,
	nvarchar_value_10 nvarchar(256) NULL,
	created_by varchar(20) NULL,
	created_time datetime NULL,
	modified_by varchar(20) NULL,
	modified_time datetime NULL
);
ALTER TABLE employees_asco4_hist
   ADD CONSTRAINT PK_employees_asco4_hist PRIMARY KEY (rec_id);

CREATE TABLE employee_responsibility
(
rec_id                    BIGINT      NOT NULL IDENTITY(1,1),
employee_id               INTEGER     NOT NULL,
unit_id                   INTEGER     NOT NULL,
type                      CHAR(2)     NOT NULL,
created_by		  VARCHAR(20)     NULL,
created_time              DATETIME        NULL,
modified_by               VARCHAR(20)     NULL,
modified_time             DATETIME        NULL
);

ALTER TABLE employee_responsibility
   ADD CONSTRAINT  PK_employee_responsibility PRIMARY KEY (rec_id);

ALTER TABLE employee_responsibility
   ADD CONSTRAINT FK_employee_responsibility_employees FOREIGN KEY (employee_id)
   REFERENCES employees (employee_id);

ALTER TABLE employee_responsibility
   ADD CONSTRAINT CHK_employee_responsibility_type CHECK (type in ('WU','OU'));


--
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Prosirenje tabele sa satnicama hdr sa atributom: flag_turnus
---------------------------------------------------------------------------------------------------------

ALTER TABLE time_schema_hdr
   ADD flag_turnus INTEGER NULL;

INSERT INTO appl_menu_items
(appl_menu_item_id, name, description,lang_code, position, permition_role_0, permition_role_1, permition_role_2, permition_role_3, permition_role_4, permition_role_5, permition_role_6, permition_role_7, permition_role_8, permition_role_9, permition_role_10, permition_role_11, permition_role_12, permition_role_13, permition_role_14, permition_role_15, permition_role_16, permition_role_17, permition_role_18, permition_role_19, permition_role_20, permition_role_21, permition_role_22, permition_role_23, permition_role_24, permition_role_25, permition_role_26, permition_role_27, permition_role_28, permition_role_29, permition_role_30, permition_role_31, permition_role_32, permition_role_33, permition_role_34, permition_role_35, permition_role_36, permition_role_37, permition_role_38, permition_role_39, created_by, created_time, modified_by, modified_time)
VALUES
(N'Izvetaji_Posebni izvetaji_FIAT', N'FIAT', N'FIAT', 'sr-SP-Latn', '020_020_055',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'SYS',GETDATE(),NULL,NULL);
INSERT INTO appl_menu_items
(appl_menu_item_id, name, description,lang_code, position, permition_role_0, permition_role_1, permition_role_2, permition_role_3, permition_role_4, permition_role_5, permition_role_6, permition_role_7, permition_role_8, permition_role_9, permition_role_10, permition_role_11, permition_role_12, permition_role_13, permition_role_14, permition_role_15, permition_role_16, permition_role_17, permition_role_18, permition_role_19, permition_role_20, permition_role_21, permition_role_22, permition_role_23, permition_role_24, permition_role_25, permition_role_26, permition_role_27, permition_role_28, permition_role_29, permition_role_30, permition_role_31, permition_role_32, permition_role_33, permition_role_34, permition_role_35, permition_role_36, permition_role_37, permition_role_38, permition_role_39, created_by, created_time, modified_by, modified_time)
VALUES
(N'Reports_Customized reports_FIAT', N'FIAT Reports', N'FIAT', 'en-US', '020_020_055',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'SYS',GETDATE(),NULL,NULL);
 
INSERT INTO appl_menu_items
(appl_menu_item_id, name, description,lang_code, position, permition_role_0, permition_role_1, permition_role_2, permition_role_3, permition_role_4, permition_role_5, permition_role_6, permition_role_7, permition_role_8, permition_role_9, permition_role_10, permition_role_11, permition_role_12, permition_role_13, permition_role_14, permition_role_15, permition_role_16, permition_role_17, permition_role_18, permition_role_19, permition_role_20, permition_role_21, permition_role_22, permition_role_23, permition_role_24, permition_role_25, permition_role_26, permition_role_27, permition_role_28, permition_role_29, permition_role_30, permition_role_31, permition_role_32, permition_role_33, permition_role_34, permition_role_35, permition_role_36, permition_role_37, permition_role_38, permition_role_39, created_by, created_time, modified_by, modified_time)
VALUES
(N'Izvetaji_Posebni izvetaji_FIAT_PY Integracija', N'PY Integracija', N'PY Integracija', 'sr-SP-Latn', '020_020_055_010',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'SYS',GETDATE(),NULL,NULL);
INSERT INTO appl_menu_items
(appl_menu_item_id, name, description,lang_code, position, permition_role_0, permition_role_1, permition_role_2, permition_role_3, permition_role_4, permition_role_5, permition_role_6, permition_role_7, permition_role_8, permition_role_9, permition_role_10, permition_role_11, permition_role_12, permition_role_13, permition_role_14, permition_role_15, permition_role_16, permition_role_17, permition_role_18, permition_role_19, permition_role_20, permition_role_21, permition_role_22, permition_role_23, permition_role_24, permition_role_25, permition_role_26, permition_role_27, permition_role_28, permition_role_29, permition_role_30, permition_role_31, permition_role_32, permition_role_33, permition_role_34, permition_role_35, permition_role_36, permition_role_37, permition_role_38, permition_role_39, created_by, created_time, modified_by, modified_time)
VALUES
(N'Reports_Customized reports_FIAT_PY Integration', N'PY Integration', N'FIAT', 'en-US', '020_020_055_010',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'SYS',GETDATE(),NULL,NULL);

--
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--Rules for showing payslips
---------------------------------------------------------------------------------------------------------

/****** Script for SelectTopNRows command from SSMS  ******/
   insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(406,-2,'PAYSLIP OFFSET VISIBILITY',1,N'Payment day offset (payslip)',2,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(407,-2,'PAYSLIP OFFSET VISIBILITY',2,N'Payment day offset (payslip)',2,'SYS',GETDATE())
   
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(408,-2,'PAYSLIP OFFSET VISIBILITY',3,N'Payment day offset (payslip)',2,'SYS',GETDATE())
      
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(409,-2,'PAYSLIP OFFSET VISIBILITY',4,N'Payment day offset (payslip)',2,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(410,-2,'PAYSLIP OFFSET VISIBILITY',5,N'Payment day offset (payslip)',2,'SYS',GETDATE())
  
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(1406,-3,'PAYSLIP OFFSET VISIBILITY',1,N'Payment day offset (payslip)',2,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(1407,-3,'PAYSLIP OFFSET VISIBILITY',2,N'Payment day offset (payslip)',2,'SYS',GETDATE())
   
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(1408,-3,'PAYSLIP OFFSET VISIBILITY',3,N'Payment day offset (payslip)',2,'SYS',GETDATE())
      
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(1409,-3,'PAYSLIP OFFSET VISIBILITY',4,N'Payment day offset (payslip)',2,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(1410,-3,'PAYSLIP OFFSET VISIBILITY',5,N'Payment day offset (payslip)',2,'SYS',GETDATE())
    
    
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(2406,-4,'PAYSLIP OFFSET VISIBILITY',1,N'Payment day offset (payslip)',2,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(2407,-4,'PAYSLIP OFFSET VISIBILITY',2,N'Payment day offset (payslip)',2,'SYS',GETDATE())
   
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(2408,-4,'PAYSLIP OFFSET VISIBILITY',3,N'Payment day offset (payslip)',2,'SYS',GETDATE())
      
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(2409,-4,'PAYSLIP OFFSET VISIBILITY',4,N'Payment day offset (payslip)',2,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(2410,-4,'PAYSLIP OFFSET VISIBILITY',5,N'Payment day offset (payslip)',2,'SYS',GETDATE())
    
    
    
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(3406,-5,'PAYSLIP OFFSET VISIBILITY',1,N'Payment day offset (payslip)',2,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(3407,-5,'PAYSLIP OFFSET VISIBILITY',2,N'Payment day offset (payslip)',2,'SYS',GETDATE())
   
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(3408,-5,'PAYSLIP OFFSET VISIBILITY',3,N'Payment day offset (payslip)',2,'SYS',GETDATE())
      
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(3409,-5,'PAYSLIP OFFSET VISIBILITY',4,N'Payment day offset (payslip)',2,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(3410,-5,'PAYSLIP OFFSET VISIBILITY',5,N'Payment day offset (payslip)',2,'SYS',GETDATE())



  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(411,-2,'PAYMENT DAY',1,N'Payment day (payslip)',28,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(412,-2,'PAYMENT DAY',2,N'Payment day (paylip)',28,'SYS',GETDATE())
   
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(413,-2,'PAYMENT DAY',3,N'Payment day (paylip)',28,'SYS',GETDATE())
      
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(414,-2,'PAYMENT DAY',4,N'Payment day (paylip)',28,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(415,-2,'PAYMENT DAY',5,N'Payment day (paylip)',28,'SYS',GETDATE())
  
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(1411,-3,'PAYMENT DAY',1,N'Payment day (paylip)',28,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(1412,-3,'PAYMENT DAY',2,N'Payment day (paylip)',28,'SYS',GETDATE())
   
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(1413,-3,'PAYMENT DAY',3,N'Payment day (paylip)',28,'SYS',GETDATE())
      
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(1414,-3,'PAYMENT DAY',4,N'Payment day (paylip)',28,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(1415,-3,'PAYMENT DAY',5,N'Payment day (paylip)',28,'SYS',GETDATE())
    
    
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(2411,-4,'PAYMENT DAY',1,N'Payment day (paylip)',28,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(2412,-4,'PAYMENT DAY',2,N'Payment day (paylip)',28,'SYS',GETDATE())
   
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(2413,-4,'PAYMENT DAY',3,N'Payment day (paylip)',28,'SYS',GETDATE())
      
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(2414,-4,'PAYMENT DAY',4,N'Payment day (paylip)',28,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(2415,-4,'PAYMENT DAY',5,N'Payment day (paylip)',28,'SYS',GETDATE())
    
    
    
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(3411,-5,'PAYMENT DAY',1,N'Payment day (paylip)',28,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(3412,-5,'PAYMENT DAY',2,N'Payment day (paylip)',28,'SYS',GETDATE())
   
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(3413,-5,'PAYMENT DAY',3,N'Payment day (paylip)',28,'SYS',GETDATE())
      
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(3414,-5,'PAYMENT DAY',4,N'Payment day (paylip)',28,'SYS',GETDATE())
  
  insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) values(3415,-5,'PAYMENT DAY',5,N'Payment day (paylip)',28,'SYS',GETDATE())

--
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
--New employee types
---------------------------------------------------------------------------------------------------------

INSERT INTO actamgr.rules 
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) 
VALUES
(10028,-6,'WCDR CUT OFF DATE',7,N'Dan u mesecu do kog je moguće menjati podatke u procesiranim parovima za WCDR',3,'SYS',GETDATE());
INSERT INTO actamgr.rules 
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) 
VALUES
(10029,-7,'WCDR CUT OFF DATE',7,N'Dan u mesecu do kog je moguće menjati podatke u procesiranim parovima za WCDR',3,'SYS',GETDATE());
INSERT INTO actamgr.rules 
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) 
VALUES
(10030,-8,'WCDR CUT OFF DATE',7,N'Dan u mesecu do kog je moguće menjati podatke u procesiranim parovima za WCDR',3,'SYS',GETDATE());
INSERT INTO actamgr.rules 
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) 
VALUES
(10031,-9,'WCDR CUT OFF DATE',7,N'Dan u mesecu do kog je moguće menjati podatke u procesiranim parovima za WCDR',3,'SYS',GETDATE());
INSERT INTO actamgr.rules 
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) 
VALUES
(10032,-9,'WCDR CUT OFF DATE',7,N'Dan u mesecu do kog je moguće menjati podatke u procesiranim parovima za WCDR',3,'SYS',GETDATE());
INSERT INTO actamgr.rules 
(rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,created_by,created_time) 
VALUES
(10033,0,'WCDR CUT OFF DATE',7,N'Dan u mesecu do kog je moguće menjati podatke u procesiranim parovima za WCDR',3,'SYS',GETDATE());


---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
-- Login Auditing 03.12.2012.
---------------------------------------------------------------------------------------------------------
ALTER TABLE appl_users_log
   ADD  login_chanel  VARCHAR(50) NOT NULL DEFAULT 'DESKTOP';

ALTER TABLE appl_users_log
   ADD CONSTRAINT CHK_appl_users_log_login_chanel  CHECK (login_chanel IN ('WEB','DESKTOP'));

ALTER TABLE appl_users_log
   ADD  login_status VARCHAR(50) NOT NULL DEFAULT 'SUCCESSFUL';

ALTER TABLE appl_users_log
   ADD CONSTRAINT CHK_appl_users_log_login_status CHECK (login_status IN ('FAILED','SUCCESSFUL'));

ALTER TABLE appl_users_log
   ADD  login_type  VARCHAR(50) NOT NULL DEFAULT 'TM';

ALTER TABLE appl_users_log
   ADD CONSTRAINT CHK_appl_users_log_login_type  CHECK (login_type IN ('TM','FIAT'));

ALTER TABLE actamgr.appl_users_log DROP CONSTRAINT FK_appl_users_log_appl_users;

CREATE TABLE appl_users_login_changes_tbl
(
rec_id                                INTEGER      NOT NULL IDENTITY (1,1),
tbl                                   VARCHAR(200) NOT NULL,
created_by		              VARCHAR(20)     NULL,
created_time                          DATETIME        NULL,
modified_by                           VARCHAR(20)     NULL,
modified_time                         DATETIME        NULL
);


ALTER TABLE appl_users_login_changes_tbl
   ADD CONSTRAINT PK_appl_users_login_changes_tbl PRIMARY KEY (rec_id);



INSERT INTO appl_menu_items
(appl_menu_item_id, name, description,lang_code, position, permition_role_0, permition_role_1, permition_role_2, permition_role_3, permition_role_4, permition_role_5, permition_role_6, permition_role_7, permition_role_8, permition_role_9, permition_role_10, permition_role_11, permition_role_12, permition_role_13, permition_role_14, permition_role_15, permition_role_16, permition_role_17, permition_role_18, permition_role_19, permition_role_20, permition_role_21, permition_role_22, permition_role_23, permition_role_24, permition_role_25, permition_role_26, permition_role_27, permition_role_28, permition_role_29, permition_role_30, permition_role_31, permition_role_32, permition_role_33, permition_role_34, permition_role_35, permition_role_36, permition_role_37, permition_role_38, permition_role_39, created_by, created_time, modified_by, modified_time)
VALUES
(N'Intervencije_Masivni unos', N'Masivni unos', N'Masivni unos', 'sr-SP-Latn', '030_080',15,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'SYS',GETDATE(),NULL,NULL);


INSERT INTO appl_menu_items
(appl_menu_item_id, name, description,lang_code, position, permition_role_0, permition_role_1, permition_role_2, permition_role_3, permition_role_4, permition_role_5, permition_role_6, permition_role_7, permition_role_8, permition_role_9, permition_role_10, permition_role_11, permition_role_12, permition_role_13, permition_role_14, permition_role_15, permition_role_16, permition_role_17, permition_role_18, permition_role_19, permition_role_20, permition_role_21, permition_role_22, permition_role_23, permition_role_24, permition_role_25, permition_role_26, permition_role_27, permition_role_28, permition_role_29, permition_role_30, permition_role_31, permition_role_32, permition_role_33, permition_role_34, permition_role_35, permition_role_36, permition_role_37, permition_role_38, permition_role_39, created_by, created_time, modified_by, modified_time)
VALUES
(N'Interventions_Massive input', N'Massive input', N'Massive input', 'en-US', '030_080',15,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'SYS',GETDATE(),NULL,NULL);


---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
-- Tabela sa zakljucanim danima ako je radjen masovan unos
---------------------------------------------------------------------------------------------------------


CREATE TABLE employee_locked_days
(
rec_id                    BIGINT      NOT NULL IDENTITY(1,1),
employee_id               INTEGER     NOT NULL,
date                      DATETIME        NOT NULL,
type                      VARCHAR(20)     NULL,               
created_by		  VARCHAR(20)     NULL,
created_time              DATETIME        NULL,
modified_by               VARCHAR(20)     NULL,
modified_time             DATETIME        NULL
);

ALTER TABLE employee_locked_days
   ADD CONSTRAINT  PK_employee_locked_days PRIMARY KEY (rec_id);

ALTER TABLE employee_locked_days
   ADD CONSTRAINT FK_employee_locked_days_employees FOREIGN KEY (employee_id)
   REFERENCES employees (employee_id);
   
---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
-- Menu item za masovni unos (samo za FIAT)
--------------------------------------------------------------------------------------------------------- 
INSERT INTO appl_menu_items
(appl_menu_item_id, name, description,lang_code, position, permition_role_0, permition_role_1, permition_role_2, permition_role_3, permition_role_4, permition_role_5, permition_role_6, permition_role_7, permition_role_8, permition_role_9, permition_role_10, permition_role_11, permition_role_12, permition_role_13, permition_role_14, permition_role_15, permition_role_16, permition_role_17, permition_role_18, permition_role_19, permition_role_20, permition_role_21, permition_role_22, permition_role_23, permition_role_24, permition_role_25, permition_role_26, permition_role_27, permition_role_28, permition_role_29, permition_role_30, permition_role_31, permition_role_32, permition_role_33, permition_role_34, permition_role_35, permition_role_36, permition_role_37, permition_role_38, permition_role_39, created_by, created_time, modified_by, modified_time)
VALUES
(N'Intervencije_Masivni unos', N'Masivni unos', N'Masivni unos', 'sr-SP-Latn', '030_080',15,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'SYS',GETDATE(),NULL,NULL);


INSERT INTO appl_menu_items
(appl_menu_item_id, name, description,lang_code, position, permition_role_0, permition_role_1, permition_role_2, permition_role_3, permition_role_4, permition_role_5, permition_role_6, permition_role_7, permition_role_8, permition_role_9, permition_role_10, permition_role_11, permition_role_12, permition_role_13, permition_role_14, permition_role_15, permition_role_16, permition_role_17, permition_role_18, permition_role_19, permition_role_20, permition_role_21, permition_role_22, permition_role_23, permition_role_24, permition_role_25, permition_role_26, permition_role_27, permition_role_28, permition_role_29, permition_role_30, permition_role_31, permition_role_32, permition_role_33, permition_role_34, permition_role_35, permition_role_36, permition_role_37, permition_role_38, permition_role_39, created_by, created_time, modified_by, modified_time)
VALUES
(N'Interventions_Massive input', N'Massive input', N'Massive input', 'en-US', '030_080',15,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'SYS',GETDATE(),NULL,NULL);


--------------------------------------------------------------------------------------------------------- 


---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
--COMMENT
-- Menu item za masovni unos (samo za FIAT)
--------------------------------------------------------------------------------------------------------- 
ALTER  TABLE employee_types
ADD CONSTRAINT PK_employee_types PRIMARY KEY (employee_type_id,working_unit_id);


CREATE TABLE employee_types_visibility
(
working_unit_id           INTEGER      NOT NULL,
employee_type_id          INTEGER      NOT NULL,
appl_users_category_id    INTEGER      NOT NULL,
value                     INTEGER      NOT NULL,
created_by		          VARCHAR(20)      NULL,
created_time              DATETIME         NULL,
modified_by               VARCHAR(20)      NULL,
modified_time             DATETIME         NULL
);

ALTER TABLE employee_types_visibility
  ADD CONSTRAINT PK_employee_types_visibility PRIMARY KEY (working_unit_id,employee_type_id,appl_users_category_id);

ALTER TABLE employee_types_visibility
   ADD CONSTRAINT CHK_employee_types_visibility_value CHECK (value IN (0,1));
    
--ALTER TABLE employee_types_visibility
--  ADD CONSTRAINT FK_employee_types_visibility_x_employee_types FOREIGN KEY (employee_type_id,working_unit_id)
--     REFERENCES employee_types (employee_type_id,working_unit_id);

ALTER TABLE employee_types_visibility
  ADD CONSTRAINT FK_employee_types_visibility_x_appl_users_categories FOREIGN KEY (appl_users_category_id)
     REFERENCES appl_users_categories (appl_users_category_id);

     
--------------------------------------------------------------------------------------------------------- 
--COMMENT
-- RULES FOR SENDING EMAILS ABOUT PAYSLIPS
--------------------------------------------------------------------------------------------------------- 

insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (456,-2,'COMPANY PAYSLIP EMAIL NOTIFICATION',1,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (457,-2,'COMPANY PAYSLIP EMAIL NOTIFICATION',2,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (458,-2,'COMPANY PAYSLIP EMAIL NOTIFICATION',3,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (459,-2,'COMPANY PAYSLIP EMAIL NOTIFICATION',4,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (460,-2,'COMPANY PAYSLIP EMAIL NOTIFICATION',5,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
          
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (1456,-3,'COMPANY PAYSLIP EMAIL NOTIFICATION',1,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (1457,-3,'COMPANY PAYSLIP EMAIL NOTIFICATION',2,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (1458,-3,'COMPANY PAYSLIP EMAIL NOTIFICATION',3,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (1459,-3,'COMPANY PAYSLIP EMAIL NOTIFICATION',4,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (1460,-3,'COMPANY PAYSLIP EMAIL NOTIFICATION',5,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())

insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (2456,-4,'COMPANY PAYSLIP EMAIL NOTIFICATION',1,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (2457,-4,'COMPANY PAYSLIP EMAIL NOTIFICATION',2,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (2458,-4,'COMPANY PAYSLIP EMAIL NOTIFICATION',3,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (2459,-4,'COMPANY PAYSLIP EMAIL NOTIFICATION',4,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (2460,-4,'COMPANY PAYSLIP EMAIL NOTIFICATION',5,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
      
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (3456,-5,'COMPANY PAYSLIP EMAIL NOTIFICATION',1,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (3457,-5,'COMPANY PAYSLIP EMAIL NOTIFICATION',2,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (3458,-5,'COMPANY PAYSLIP EMAIL NOTIFICATION',3,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (3459,-5,'COMPANY PAYSLIP EMAIL NOTIFICATION',4,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())
insert into actamgr.rules (rule_id,working_unit_id,rule_type,employee_type_id,rule_description,rule_value,rule_datetime_1,rule_datetime_2,created_by,created_time) values (3460,-5,'COMPANY PAYSLIP EMAIL NOTIFICATION',5,'Da li treba poslati mejlove za obračunske listove',1,'1900-01-01','1900-01-01','SYS',GETDATE())

---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------

CREATE TABLE system_closing_events
(
event_id         INTEGER      NOT NULL IDENTITY (1,1),
type             VARCHAR(30)  NOT NULL,
start_time       DATETIME         NULL,
end_time         DATETIME         NULL,
state_flagged    INTEGER          NULL,
status           VARCHAR(30)  NOT NULL,
message_sr       NVARCHAR(2000) NOT  NULL,
message_en       NVARCHAR(2000) NOT  NULL,
processing_engine_state  INTEGER NOT NULL,
created_by		VARCHAR(20)    			,
created_time    DATETIME       			,
modified_by		VARCHAR(20)             ,
modified_time   DATETIME
);
ALTER TABLE system_closing_events
   ADD CONSTRAINT PK_system_closing_events PRIMARY KEY (event_id);

ALTER TABLE system_closing_events
   ADD CONSTRAINT  CHK_system_closing_events_type CHECK (type IN ('FLAGGED','INTERVAL'));
   
ALTER TABLE system_closing_events
   ADD CONSTRAINT  CHK_system_closing_events_status CHECK (status IN ('ACTIVE','DISSMISSED'));
   
ALTER TABLE system_closing_events
   ADD CONSTRAINT  CHK_system_closing_events_processing_engine_state CHECK (processing_engine_state IN (0,1));

CREATE TABLE system_msgs
(
msg_id          INTEGER      NOT NULL IDENTITY (1,1),
working_unit_id INTEGER      NOT NULL,
appl_users_category_id INTEGER      NOT NULL,
type             VARCHAR(30)  NOT NULL,
start_time       DATETIME         NULL,
end_time         DATETIME         NULL,
state_flagged    INTEGER          NULL,
status           VARCHAR(30)  NOT NULL,
message_sr       NVARCHAR(2000) NOT  NULL,
message_en       NVARCHAR(2000) NOT  NULL,
created_by		VARCHAR(20)    			,
created_time    DATETIME       			,
modified_by		VARCHAR(20)             ,
modified_time   DATETIME
);
ALTER TABLE system_msgs
   ADD CONSTRAINT PK_system_msgs PRIMARY KEY (msg_id);

ALTER TABLE system_msgs
   ADD CONSTRAINT  CHK_system_msgs_type CHECK (type IN ('FLAGGED','INTERVAL'));
   
ALTER TABLE system_msgs
   ADD CONSTRAINT  CHK_system_msgs_status CHECK (status IN ('ACTIVE','DISSMISSED'));
   
ALTER TABLE system_msgs
   ADD CONSTRAINT  FK_system_msgs_x_working_units FOREIGN KEY (working_unit_id)
     REFERENCES working_units (working_unit_id);

ALTER TABLE system_msgs
   ADD CONSTRAINT  FK_system_msgs_x_appl_users_categories FOREIGN KEY (appl_users_category_id)
     REFERENCES appl_users_categories (appl_users_category_id);
