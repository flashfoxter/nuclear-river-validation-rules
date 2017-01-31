﻿if not exists (select * from sys.schemas where name = 'ThemeAggregates') exec('create schema ThemeAggregates')
go

if object_id('ThemeAggregates.Theme') is not null drop table ThemeAggregates.Theme
if object_id('ThemeAggregates.InvalidCategory') is not null drop table ThemeAggregates.InvalidCategory

if object_id('ThemeAggregates.Order') is not null drop table ThemeAggregates.[Order]
if object_id('ThemeAggregates.OrderTheme') is not null drop table ThemeAggregates.OrderTheme

if object_id('ThemeAggregates.Project') is not null drop table ThemeAggregates.Project
if object_id('ThemeAggregates.ProjectDefaultTheme') is not null drop table ThemeAggregates.ProjectDefaultTheme

go

-- Theme aggregate
create table ThemeAggregates.Theme (
    Id bigint not null,

    BeginDistribution datetime2(2) not null,
    EndDistribution datetime2(2) not null,

    IsDefault bit not null,

    constraint PK_Theme primary key (Id)
)
go
create table ThemeAggregates.InvalidCategory (
    ThemeId bigint not null,
    CategoryId bigint not null,
)
go

-- Order aggregate
create table ThemeAggregates.[Order] (
    Id bigint not null,

    BeginDistributionDate datetime2(2) not null,
    EndDistributionDateFact datetime2(2) not null,

    ProjectId bigint not null,

    IsSelfAds bit not null,
    constraint PK_Order primary key (Id)
)
go
create table ThemeAggregates.OrderTheme (
    OrderId bigint not null,
    ThemeId bigint not null,
)
go

-- Project aggregate
create table ThemeAggregates.Project (
    Id bigint not null,

    constraint PK_Project primary key (Id)
)
go

create table ThemeAggregates.ProjectDefaultTheme (
    ProjectId bigint not null,
    ThemeId bigint not null,
    [Start] datetime2(2) not null,
    [End] datetime2(2) not null,
)
go

go
