--Data about finacial structure
-------------------------------------------------
CREATE TABLE sync_financial_structure_hist
(
rec_hist_id                          INTEGER IDENTITY(1,1) NOT NULL,
rec_id                               INTEGER      NOT NULL,
unit_id                              INTEGER      NOT NULL,
company_code                         VARCHAR(10)      NULL,
unit_stringone                       VARCHAR(50)      NULL,
description                          NVARCHAR(50)     NULL,
valid_from                           DATETIME         NULL,
created_by                           VARCHAR(20)  NOT NULL,
created_time                         DATETIME     NOT NULL,
created_time_hist                    DATETIME     NOT NULL,
result                               INTEGER      NOT NULL,
remark                               NVARCHAR(2000)   NULL
);

ALTER TABLE sync_financial_structure_hist
  ADD CONSTRAINT PK_sync_financial_structure_hist PRIMARY KEY (rec_hist_id);
  

--Data about organizational structure
-------------------------------------------------
CREATE TABLE sync_organizational_structure_hist
(
rec_hist_id                          INTEGER IDENTITY(1,1) NOT NULL,
rec_id                               INTEGER      NOT NULL,
unit_id                              INTEGER      NOT NULL,
parent_unit_id                       INTEGER          NULL,
description                          NVARCHAR(50)     NULL,
code                                 VARCHAR(30)      NULL,
cost_center_code                     VARCHAR(50)      NULL,
cost_center_company_code             VARCHAR(10)      NULL,
valid_from                           DATETIME         NULL,
created_by                           VARCHAR(20)  NOT NULL,
created_time                         DATETIME     NOT NULL,
created_time_hist                    DATETIME     NOT NULL,
result                               INTEGER      NOT NULL,
remark                               NVARCHAR(2000)   NULL
);

ALTER TABLE sync_organizational_structure_hist
  ADD CONSTRAINT PK_sync_organizational_structure_hist PRIMARY KEY (rec_hist_id);


--Data about organizational employees
-------------------------------------------------
CREATE TABLE sync_employees_hist
(
rec_hist_id                          INTEGER IDENTITY(1,1) NOT NULL,
rec_id                               INTEGER      NOT NULL,
employee_id                          INTEGER      NOT NULL,
employee_id_old                      NVARCHAR(max)   NULL,
first_name                           NVARCHAR(50)     NULL,
last_name                            NVARCHAR(50)     NULL,
tag_id                               BIGINT           NULL,
fs_unit_id                           INTEGER          NULL,
picture                              IMAGE            NULL,
employee_type_id                     INTEGER          NULL,
personal_holiday_category            INTEGER          NULL,
personal_holiday_date                DATETIME         NULL,
organizational_unit_id               INTEGER          NULL,
responsibility_fs_unit_id            INTEGER          NULL,
responsibility_ou_unit_id            INTEGER          NULL,
email_address                        VARCHAR(max)     NULL,
JMBG                                 VARCHAR(50)      NULL,       
username                             VARCHAR(100)     NULL, 
status                               VARCHAR(20)      NULL,           
employee_branch                      CHAR(1)          NULL,
annual_leave_current_year            INTEGER          NULL,
annual_leave_previous_year           INTEGER          NULL,
language                             VARCHAR(10)      NULL,
valid_from                           DATETIME         NULL,
created_by                           VARCHAR(20)  NOT NULL,
created_time                         DATETIME     NOT NULL,
created_time_hist                    DATETIME     NOT NULL,
result                               INTEGER      NOT NULL,
remark                               NVARCHAR(2000)   NULL
); 

ALTER TABLE sync_employees_hist
  ADD CONSTRAINT PK_sync_employees_hist PRIMARY KEY (rec_hist_id);

ALTER TABLE sync_employees_hist
  ADD CONSTRAINT CHK_sync_employees_hist CHECK (language IN ('SRB','ENG'));
/*  
--Data about wages types
-------------------------------------------------
CREATE TABLE sync_wages_types
(
rec_id                                  INTEGER IDENTITY(1,1) NOT NULL,
payroll_item_id			        INTEGER		 NOT NULL,
payroll_Item_Code                       CHAR(10)         NULL,
is_percentage                           BIT              NULL,
is_fixed_amount                         BIT              NULL,
max_value                               DECIMAL(15,7)    NULL,
min_value                               DECIMAL(15,7)    NULL,
plus_or_minus                           BIT		 NULL,
is_taxable                              BIT		 NULL, 
tax_payroll_item_id                     INTEGER          NULL,       
accounting_code                         VARCHAR(50)      NULL,           
bonus_payroll_item                      BIT              NULL,
is_basic_payroll_item			BIT	         NULL,
item_default_value                      DECIMAL(18,4)	 NULL,
id_payRoll_wage_type			BIGINT	         NOT NULL,
id_wagetype_vrsta                       INT	         NULL,
koristi_se_za_obracun_zarada            BIT              NULL,
koristi_se_za_TM                        BIT              NULL,
procenat_osnovne_zarade_pri_obracunu    NVARCHAR(50)     NULL,
vrsta_obracuna                          NVARCHAR(50)     NULL,
obracunava_se_minul_rad                 BIT              NULL,
ulazi_u_redovan_fond_sati               BIT              NULL,
broj_meseci_proseka                     INTEGER          NULL,
sati_ulaze_u_prosek_tromesecnog_proseka	BIT              NULL,
koristi_se_u_obracun_prevoza            BIT              NULL,
koristi_se_u_obracun_toplog_obroka      BIT              NULL,
automatski_unos_sa_terminala            BIT              NULL,
rucni_unos                              BIT              NULL,
WC_supervizor_provera                   BIT              NULL,
WC_samostalni_unos                      BIT              NULL,
HRSSC                                   BIT              NULL,
TL                                      BIT              NULL,
counter                                 BIT              NULL,
odsustva                                NVARCHAR(250)    NULL,
); 

ALTER TABLE sync_wages_types
  ADD CONSTRAINT PK_sync_wages_types PRIMARY KEY (rec_id);
*/
  
  --Data about wages types
-------------------------------------------------
CREATE TABLE sync_buffer_data_hist
(
rec_id                                  INTEGER IDENTITY(1,1) NOT NULL,
rec_hist_id                             INTEGER      NOT NULL,
employee_id                             INTEGER      NOT NULL,
days_of_holiday_for_current_years_left	INTEGER			 NULL,
Days_of_holiday_for_previous_years_left	INTEGER			 NULL,
created_by                              VARCHAR(20)  NOT NULL,
created_time                            DATETIME     NOT NULL
); 

ALTER TABLE sync_buffer_data_hist
  ADD CONSTRAINT PK_sync_buffer_data_hist PRIMARY KEY (rec_id);


 --Data about sync_responsibility_hist
-------------------------------------------------
CREATE TABLE sync_responsibility_hist
(
rec_hist_id               INTEGER     NOT NULL IDENTITY(1,1),
rec_id                    int NOT NULL,
responibility_id          int NULL,
unit_id                   int NULL,
responsible_person_id     int NULL,
structure_type            char(1) NULL,
valid_from                datetime NULL,
valid_to                  datetime NULL,
created_by                varchar(20) NOT NULL,
created_time              datetime NOT NULL,
created_time_hist         DATETIME     NOT NULL,
result                    INTEGER      NOT NULL,
remark                    NVARCHAR(2000)   NULL
);

ALTER TABLE sync_responsibility_hist
   ADD CONSTRAINT  PK_sync_responsibility_hist PRIMARY KEY (rec_hist_id );


  --Data about sync_employee_positions
-------------------------------------------------
CREATE TABLE sync_employee_positions_hist
(
rec_hist_id               INTEGER     NOT NULL IDENTITY(1,1),
rec_id                    int NOT NULL,
company_code             VARCHAR(30)               NULL,
status                   NVARCHAR(50)              NULL,
position_id              INTEGER         NOT NULL,
position_code            NVARCHAR(20)        NULL,
position_title_sr        NVARCHAR(MAX)       NULL,
position_title_en        NVARCHAR(MAX)       NULL,
desc_sr                  NVARCHAR(MAX)      NULL,
desc_en                  NVARCHAR(MAX)      NULL,
valid_from               DATETIME        NOT NULL,
created_by		  VARCHAR(20)        NULL,
created_time    	 DATETIME            NULL,
created_time_hist         DATETIME     NOT NULL,
result                    INTEGER      NOT NULL,
remark                    NVARCHAR(2000)   NULL
);

ALTER TABLE sync_employee_positions_hist
  ADD CONSTRAINT PK_sync_employee_positions_hist PRIMARY KEY (rec_hist_id);


