if object_id('dbo.MealEntries') is null
create table dbo.MealEntries
(
	Id int not null identity(1, 1),
	[Date] date not null,
	Type nvarchar(100) not null,
	Calories int not null,
	Carbs int not null,
	Fats int not null,
	Proteins int not null,
	Comments nvarchar(1000),

	CreatedAt datetime not null default(getutcdate()),
	constraint PK_MealEntries primary key(Id)
)