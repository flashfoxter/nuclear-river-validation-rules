﻿if not exists (select * from sys.schemas where name = 'PriceContext') exec('create schema PriceContext')
go

if object_id('PriceContext.AssociatedPosition') is not null drop table PriceContext.AssociatedPosition
if object_id('PriceContext.AssociatedPositionsGroup') is not null drop table PriceContext.AssociatedPositionsGroup
if object_id('PriceContext.DeniedPosition') is not null drop table PriceContext.DeniedPosition
if object_id('PriceContext.OrderPositionAdvertisement') is not null drop table PriceContext.OrderPositionAdvertisement
if object_id('PriceContext.OrderPosition') is not null drop table PriceContext.OrderPosition
if object_id('PriceContext.Order') is not null drop table PriceContext.[Order]
if object_id('PriceContext.OrganizationUnit') is not null drop table PriceContext.OrganizationUnit
if object_id('PriceContext.PricePosition') is not null drop table PriceContext.PricePosition
if object_id('PriceContext.Price') is not null drop table PriceContext.Price
if object_id('PriceContext.Project') is not null drop table PriceContext.Project
if object_id('PriceContext.Position') is not null drop table PriceContext.Position
if object_id('PriceContext.Category') is not null drop table PriceContext.Category
if object_id('PriceContext.GlobalAssociatedPosition') is not null drop table PriceContext.GlobalAssociatedPosition
if object_id('PriceContext.GlobalDeniedPosition') is not null drop table PriceContext.GlobalDeniedPosition
go

create table PriceContext.Price(
    Id bigint not null,
    OrganizationUnitId bigint not null,
    BeginDate datetime2(2) not null,
    constraint PK_Price primary key (Id)
)
go

create table PriceContext.Position(
    Id bigint not null,
    PositionCategoryId bigint not null, -- в erm назван CategoryId
    IsControlledByAmount bit not null,
    IsComposite bit not null,
    CompareMode int not null,
    Name nvarchar(max) not null,
    constraint PK_Position primary key (Id)
)
create index IX_IsComposite ON PriceContext.Position (IsComposite)
go

create table PriceContext.PricePosition(
    Id bigint not null,
    PriceId bigint not null,
    PositionId bigint not null,
    MinAdvertisementAmount int not null,
    MaxAdvertisementAmount int not null,
    constraint PK_PricePosition primary key (Id)
)
create index IX_PriceId ON PriceContext.PricePosition (PriceId)
create index IX_PositionId ON PriceContext.PricePosition (PositionId)
go

create table PriceContext.AssociatedPositionsGroup(
    Id bigint not null,
    PricePositionId bigint not null,
    constraint PK_AssociatedPositionsGroup primary key (Id)
)
create index IX_PricePositionId ON PriceContext.AssociatedPositionsGroup (PricePositionId)
go

create table PriceContext.AssociatedPosition(
    Id bigint not null,
    AssociatedPositionsGroupId bigint not null,
    PositionId bigint not null,
    ObjectBindingType int not null,
    constraint PK_AssociatedPosition primary key (Id)
)
create index IX_AssociatedPositionsGroupId ON PriceContext.AssociatedPosition (AssociatedPositionsGroupId)
go

create table PriceContext.DeniedPosition(
    Id bigint not null,
    PositionId bigint not null,
    PositionDeniedId bigint not null,
    ObjectBindingType int not null,
    PriceId bigint not null,
    constraint PK_DeniedPosition primary key (Id)
)
go

create table PriceContext.[Order](
    Id bigint not null,
    FirmId bigint not null,
    DestOrganizationUnitId bigint not null,
    SourceOrganizationUnitId bigint not null,
    OwnerId bigint not null,
    BeginDistributionDate datetime2(2) not null,
    EndDistributionDateFact datetime2(2) not null,
    BeginReleaseNumber int not null,
    EndReleaseNumberFact int not null,
    EndReleaseNumberPlan int not null,
    WorkflowStepId int not null,
    Number nvarchar(max) not null,
    constraint PK_Order primary key (Id)
)
go

create table PriceContext.OrderPosition(
    Id bigint not null,
    OrderId bigint not null,
    PricePositionId bigint not null,
    constraint PK_OrderPosition primary key (Id)
)
create index IX_OrderId ON PriceContext.OrderPosition (OrderId)
create index IX_PricePositionId ON PriceContext.OrderPosition (PricePositionId)
go

create table PriceContext.OrderPositionAdvertisement(
    Id bigint not null,
    OrderPositionId bigint not null,
    PositionId bigint not null,
    CategoryId bigint null,
    FirmAddressId bigint null,
    constraint PK_OrderPositionAdvertisement primary key (Id)
)
create index IX_OrderPositionId ON PriceContext.OrderPositionAdvertisement (OrderPositionId)
create index IX_PositionId ON PriceContext.OrderPositionAdvertisement (PositionId)
go

create table PriceContext.OrganizationUnit(
    Id bigint not null,
    constraint PK_OrganizationUnit primary key (Id)
)
go

create table PriceContext.Project(
    Id bigint not null,
    OrganizationUnitId bigint not null,
    constraint PK_Project primary key (Id)
)
go

create table PriceContext.Category(
    Id bigint not null,
    ParentId bigint not null,
    constraint PK_Category primary key (Id)
)
create index IX_ParentId ON PriceContext.Category (ParentId)
go

create table PriceContext.GlobalAssociatedPosition(
	Id bigint not null,
	RulesetId bigint not null,
    AssociatedPositionId bigint not null,
    PrincipalPositionId bigint not null,
    ObjectBindingType int not null,
	constraint PK_GlobalAssociatedPosition primary key (Id)
)
go

create table PriceContext.GlobalDeniedPosition(
	Id bigint not null,
	RulesetId bigint not null,
    DeniedPositionId bigint not null,
    PrincipalPositionId bigint not null,
    ObjectBindingType int not null,
	constraint PK_GlobalDeniedPosition primary key (Id)
)
go
