﻿
CREATE TABLE public."Setup"
(
    id bigserial NOT NULL,
    "stPhoneMask" character varying(99),
	"stDateFormat" character varying(99),
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Setup"
    OWNER to postgres;

CREATE TABLE public."Profile"
(
    id bigserial NOT NULL,
    "stName" character varying(200),
    "stPermissions" character varying(9999),
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Profile"
    OWNER to postgres;

CREATE TABLE public."User"
(
    id bigserial NOT NULL,
    "stLogin" character varying(200),
    "stPassword" character varying(30),
    "bActive" boolean,
    "fkProfile" bigint,
    "dtLastLogin" timestamp without time zone,
    "dtCreation" timestamp without time zone,
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."User"
    OWNER to postgres;

CREATE TABLE public."UserEmail"
(
    id bigserial NOT NULL,
    "fkUser" bigint,
    "stEmail" character varying(250),
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."UserEmail"
    OWNER to postgres;

CREATE TABLE public."UserPhone"
(
    id bigserial NOT NULL,
    "fkUser" bigint,
    "stPhone" character varying(50),
    "stDescription" character varying(50),
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."UserPhone"
    OWNER to postgres;

CREATE TABLE public."Project"
(
    id bigserial NOT NULL,
    "stName" character varying(99),
	"fkUser" bigint,
	"dtCreation" timestamp without time zone,
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Project"
    OWNER to postgres;

CREATE TABLE public."ProjectUser"
(
    id bigserial NOT NULL,
    "fkProject" bigint,
    "fkUser" bigint,
	"stRole" character varying(99),
	"dtJoin" timestamp without time zone,
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."ProjectUser"
    OWNER to postgres;

CREATE TABLE public."ProjectPhase"
(
    id bigserial NOT NULL,
    "fkProject" bigint,
	"stName" character varying(99),
	"dtStart" timestamp without time zone,
	"dtEnd" timestamp without time zone,
	"bComplete" boolean,
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."ProjectPhase"
    OWNER to postgres;

CREATE TABLE public."ProjectSprint"
(
    id bigserial NOT NULL,
    "fkProject" bigint,
	"fkPhase" bigint,
	"stName" character varying(200),
	"stDescription" character varying(1000),
	"dtStart" timestamp without time zone,
	"dtEnd" timestamp without time zone,
	"bComplete" boolean,
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."ProjectSprint"
    OWNER to postgres;
	
CREATE TABLE public."TaskType"
(
    id bigserial NOT NULL,
    "stName" character varying(200),
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."TaskType"
    OWNER to postgres;


CREATE TABLE public."TaskCategory"
(
    id bigserial NOT NULL,
	"fkTaskType" bigint,
    "stName" character varying(200),
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."TaskCategory"
    OWNER to postgres;


CREATE TABLE public."TaskFlow"
(
    id bigserial NOT NULL,
	"fkTaskType" bigint,
    "stName" character varying(200),
	"nuOrder" bigint,
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."TaskFlow"
    OWNER to postgres;
	

CREATE TABLE public."ProjectSprintVersion"
(
    id bigserial NOT NULL,
    "fkSprint" bigint,
	"stName" character varying(20),
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."ProjectSprintVersion"
    OWNER to postgres;
