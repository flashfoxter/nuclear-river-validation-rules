-- drop views
if object_id('ERM.ViewClient', 'view') is not null drop view ERM.ViewClient;
if object_id('BIT.FirmCategory', 'view') is not null drop view BIT.FirmCategory;
if object_id('CustomerIntelligence.FirmCategory', 'view') is not null drop view CustomerIntelligence.FirmCategory;
if object_id('CustomerIntelligence.FirmView', 'view') is not null drop view CustomerIntelligence.FirmView;
go