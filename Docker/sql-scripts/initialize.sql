CREATE TABLE epsilon.LearningDomains (
    Id VARCHAR(255) PRIMARY KEY,
    RowsSetId VARCHAR(255),
    ColumnsSetId VARCHAR(255),
    ValuesSetId VARCHAR(255)
);

-- Create the LearningDomainTypes table
CREATE TABLE epsilon.LearningDomainTypes (
    Id VARCHAR(255) PRIMARY KEY,
    Name VARCHAR(255),
    ShortName VARCHAR(255),
    HexColor VARCHAR(255)
);

-- Create the LearningDomainTypeSet table
CREATE TABLE epsilon.LearningDomainTypeSet (
    Id VARCHAR(255) PRIMARY KEY
);

-- Create the LearningDomainTypeSetTypes table
CREATE TABLE epsilon.LearningDomainTypeSetTypes (
    SetsId VARCHAR(255),
    TypesId VARCHAR(255),
    FOREIGN KEY (SetsId) REFERENCES epsilon.LearningDomainTypeSet(Id),
    FOREIGN KEY (TypesId) REFERENCES epsilon.LearningDomainTypes(Id)
);

-- Create the LearningDomainOutcomes table
CREATE TABLE epsilon.LearningDomainOutcomes (
    Id INT PRIMARY KEY,
    TenantId VARCHAR(255),
    RowId VARCHAR(255),
    ColumnId VARCHAR(255),
    ValueId VARCHAR(255),
    Name VARCHAR(255)
);


INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6773, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_one', 'Analysis-H1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6774, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_one', 'Analysis-H1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6775, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_one', 'Analysis-H1.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6776, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_two', 'Analysis-H2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6777, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_two', 'Analysis-H2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6778, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_two', 'Analysis-H2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6779, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_two', 'Analysis-H2.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6780, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_three', 'Analysis-H3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6781, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_three', 'Analysis-H3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6782, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_three', 'Analysis-H3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6783, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_four', 'Analysis-H4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6784, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'analysis', 'level_four', 'Analysis-H4.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6785, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'advise', 'level_one', 'Advise-H1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6786, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'advise', 'level_one', 'Advise-H1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6787, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'advise', 'level_two', 'Advise-H2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6788, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'advise', 'level_two', 'Advise-H2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6789, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'advise', 'level_three', 'Advise-H3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6790, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'advise', 'level_four', 'Advise-H4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6791, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'advise', 'level_four', 'Advise-H4.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6792, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'advise', 'level_four', 'Advise-H4.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6801, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'design', 'level_one', 'Design-H1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6802, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'design', 'level_two', 'Design-H2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6803, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'design', 'level_three', 'Design-H3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6804, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'design', 'level_four', 'Design-H4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6805, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'realisation', 'level_one', 'Realisation-H1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6806, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'realisation', 'level_two', 'Realisation-H2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6807, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'realisation', 'level_two', 'Realisation-H2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6808, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'realisation', 'level_two', 'Realisation-H2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6809, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'realisation', 'level_three', 'Realisation-H3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6810, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'realisation', 'level_three', 'Realisation-H3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6811, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'realisation', 'level_four', 'Realisation-H4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6812, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'manage_control', 'level_one', 'Manage&Control-H1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6813, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'manage_control', 'level_two', 'Manage&Control-H2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6814, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'manage_control', 'level_two', 'Manage&Control-H2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6815, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'manage_control', 'level_three', 'Manage&Control-H3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6816, '08473358-8c16-4375-bdeb-d468dfeb024a', 'hardware_interfacing', 'manage_control', 'level_four', 'Manage&Control-H4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6817, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'analysis', 'level_one', 'Analysis-S1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6818, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'analysis', 'level_one', 'Analysis-S1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6819, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'analysis', 'level_two', 'Analysis-S2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6820, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'analysis', 'level_two', 'Analysis-S2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6821, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'analysis', 'level_two', 'Analysis-S2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6822, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'analysis', 'level_three', 'Analysis-S3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6823, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'analysis', 'level_three', 'Analysis-S3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6824, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'analysis', 'level_four', 'Analysis-S4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6825, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'advise', 'level_one', 'Advise-S1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6826, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'advise', 'level_two', 'Advise-S2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6827, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'advise', 'level_two', 'Advise-S2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6828, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'advise', 'level_two', 'Advise-S2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6829, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'advise', 'level_three', 'Advise-S3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6830, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'advise', 'level_three', 'Advise-S3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6831, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'advise', 'level_three', 'Advise-S3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6832, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'advise', 'level_four', 'Advise-S4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6833, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'design', 'level_one', 'Design-S1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6834, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'design', 'level_two', 'Design-S2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6835, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'design', 'level_two', 'Design-S2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6836, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'design', 'level_two', 'Design-S2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6837, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'design', 'level_two', 'Design-S2.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6838, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'design', 'level_two', 'Design-S2.5');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6839, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'design', 'level_three', 'Design-S3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6840, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'design', 'level_three', 'Design-S3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6841, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'design', 'level_four', 'Design-S4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6842, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'design', 'level_four', 'Design-S4.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6843, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'realisation', 'level_one', 'Realisation-S1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6844, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'realisation', 'level_two', 'Realisation-S2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6845, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'realisation', 'level_two', 'Realisation-S2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6846, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'realisation', 'level_two', 'Realisation-S2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6847, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'realisation', 'level_three', 'Realisation-S3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6848, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'realisation', 'level_three', 'Realisation-S3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6849, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'realisation', 'level_four', 'Realisation-S4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6850, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'realisation', 'level_four', 'Realisation-S4.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6851, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'manage_control', 'level_one', 'Manage&Control-S1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6852, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'manage_control', 'level_two', 'Manage&Control-S2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6853, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'manage_control', 'level_two', 'Manage&Control-S2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6854, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'manage_control', 'level_three', 'Manage&Control-S3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6855, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'manage_control', 'level_three', 'Manage&Control-S3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6856, '08473358-8c16-4375-bdeb-d468dfeb024a', 'software', 'manage_control', 'level_four', 'Manage&Control-S4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6857, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'analysis', 'level_one', 'Analysis-I1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6858, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'analysis', 'level_two', 'Analysis-I2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6859, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'analysis', 'level_two', 'Analysis-I2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6860, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'analysis', 'level_three', 'Analysis-I3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6861, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'analysis', 'level_three', 'Analysis-I3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6862, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'analysis', 'level_four', 'Analysis-I4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6863, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'advise', 'level_one', 'Advise-I1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6864, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'advise', 'level_two', 'Advise-I2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6865, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'advise', 'level_two', 'Advise-I2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6866, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'advise', 'level_three', 'Advise-I3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6867, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'advise', 'level_three', 'Advise-I3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6868, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'advise', 'level_four', 'Advise-I4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6869, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'design', 'level_one', 'Design-I1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6870, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'design', 'level_two', 'Design-I2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6871, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'design', 'level_two', 'Design-I2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6872, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'design', 'level_two', 'Design-I2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6873, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'design', 'level_three', 'Design-I3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6874, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'design', 'level_three', 'Design-I3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6875, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'design', 'level_four', 'Design-I4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6876, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'realisation', 'level_one', 'Realisation-I1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6877, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'realisation', 'level_two', 'Realisation-I2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6878, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'realisation', 'level_two', 'Realisation-I2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6879, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'realisation', 'level_two', 'Realisation-I2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6880, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'realisation', 'level_three', 'Realisation-I3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6881, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'realisation', 'level_three', 'Realisation-I3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6882, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'realisation', 'level_three', 'Realisation-I3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6883, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'realisation', 'level_four', 'Realisation-I4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6884, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'realisation', 'level_four', 'Realisation-I4.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6885, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'manage_control', 'level_one', 'Manage&Control-I1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6886, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'manage_control', 'level_two', 'Manage&Control-I2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6887, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'manage_control', 'level_two', 'Manage&Control-I2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6888, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'manage_control', 'level_two', 'Manage&Control-I2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6889, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'manage_control', 'level_three', 'Manage&Control-I3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6890, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'manage_control', 'level_three', 'Manage&Control-I3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6891, '08473358-8c16-4375-bdeb-d468dfeb024a', 'infrastructure', 'manage_control', 'level_four', 'Manage&Control-I4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6892, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'analysis', 'level_one', 'Analysis-O1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6893, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'analysis', 'level_one', 'Analysis-O1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6894, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'analysis', 'level_two', 'Analysis-O2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6895, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'analysis', 'level_two', 'Analysis-O2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6896, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'analysis', 'level_two', 'Analysis-O2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6897, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'analysis', 'level_two', 'Analysis-O2.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6898, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'analysis', 'level_three', 'Analysis-O3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6899, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'analysis', 'level_three', 'Analysis-O3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6900, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'analysis', 'level_three', 'Analysis-O3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6901, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'analysis', 'level_three', 'Analysis-O3.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6902, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'analysis', 'level_four', 'Analysis-O4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6903, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'advise', 'level_one', 'Advise-O1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6904, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'advise', 'level_two', 'Advise-O2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6905, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'advise', 'level_two', 'Advise-O2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6906, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'advise', 'level_three', 'Advise-O3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6907, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'advise', 'level_three', 'Advise-O3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6908, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'advise', 'level_three', 'Advise-O3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6909, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'advise', 'level_four', 'Advise-O4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6910, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'advise', 'level_four', 'Advise-O4.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6911, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'design', 'level_one', 'Design-O1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6912, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'design', 'level_two', 'Design-O2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6913, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'design', 'level_two', 'Design-O2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6914, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'design', 'level_two', 'Design-O2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6915, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'design', 'level_three', 'Design-O3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6916, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'design', 'level_three', 'Design-O3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6917, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'design', 'level_three', 'Design-O3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6918, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'design', 'level_four', 'Design-O4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6919, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'design', 'level_four', 'Design-O4.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6920, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'realisation', 'level_one', 'Realisation-O1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6921, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'realisation', 'level_one', 'Realisation-O1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6922, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'realisation', 'level_one', 'Realisation-O1.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6923, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'realisation', 'level_two', 'Realisation-O2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6924, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'realisation', 'level_two', 'Realisation-O2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6925, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'realisation', 'level_two', 'Realisation-O2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6926, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'realisation', 'level_two', 'Realisation-O2.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6927, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'realisation', 'level_three', 'Realisation-O3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6928, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'realisation', 'level_three', 'Realisation-O3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6929, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'realisation', 'level_four', 'Realisation-O4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6930, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'manage_control', 'level_one', 'Manage&Control-O1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6931, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'manage_control', 'level_one', 'Manage&Control-O1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6932, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'manage_control', 'level_two', 'Manage&Control-O2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6933, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'manage_control', 'level_two', 'Manage&Control-O2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6934, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'manage_control', 'level_three', 'Manage&Control-O3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6935, '08473358-8c16-4375-bdeb-d468dfeb024a', 'organisational_processes', 'manage_control', 'level_three', 'Manage&Control-O3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6936, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'analysis', 'level_one', 'Analysis-U1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6937, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'analysis', 'level_one', 'Analysis-U1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6938, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'analysis', 'level_one', 'Analysis-U1.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6939, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'analysis', 'level_two', 'Analysis-U2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6940, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'analysis', 'level_two', 'Analysis-U2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6941, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'analysis', 'level_two', 'Analysis-U2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6942, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'analysis', 'level_three', 'Analysis-U3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6943, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'analysis', 'level_three', 'Analysis-U3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6944, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'analysis', 'level_three', 'Analysis-U3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6945, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'analysis', 'level_four', 'Analysis-U4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6946, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'advise', 'level_one', 'Advise-U1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6947, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'advise', 'level_one', 'Advise-U1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6948, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'advise', 'level_two', 'Advise-U2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6949, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'advise', 'level_two', 'Advise-U2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6950, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'advise', 'level_two', 'Advise-U2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6951, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'advise', 'level_three', 'Advise-U3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6952, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'advise', 'level_three', 'Advise-U3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6953, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'advise', 'level_four', 'Advise-U4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6954, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'design', 'level_one', 'Design-U1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6955, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'design', 'level_one', 'Design-U1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6956, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'design', 'level_one', 'Design-U1.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6957, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'design', 'level_two', 'Design-U2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6958, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'design', 'level_two', 'Design-U2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6959, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'design', 'level_three', 'Design-U3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6960, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'design', 'level_three', 'Design-U3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6961, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'design', 'level_four', 'Design-U4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6962, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'realisation', 'level_one', 'Realisation-U1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6963, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'realisation', 'level_two', 'Realisation-U2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6964, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'realisation', 'level_two', 'Realisation-U2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6965, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'realisation', 'level_two', 'Realisation-U2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6966, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'realisation', 'level_three', 'Realisation-U3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6967, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'realisation', 'level_three', 'Realisation-U3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6968, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'realisation', 'level_four', 'Realisation-U4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6969, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'realisation', 'level_four', 'Realisation-U4.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6970, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'manage_control', 'level_one', 'Manage&Control-U1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6971, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'manage_control', 'level_two', 'Manage&Control-U2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6972, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'manage_control', 'level_two', 'Manage&Control-U2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6973, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'manage_control', 'level_three', 'Manage&Control-U3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6974, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'manage_control', 'level_three', 'Manage&Control-U3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6975, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'manage_control', 'level_three', 'Manage&Control-U3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (6976, '08473358-8c16-4375-bdeb-d468dfeb024a', 'user_interaction', 'manage_control', 'level_four', 'Manage&Control-U4.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11460, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_two', 'FOO-2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11462, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_two', 'FOO-2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11463, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_two', 'FOO-2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11464, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_two', 'FOO-2.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11465, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_two', 'FOO-2.5');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11466, '08473358-8c16-4375-bdeb-d468dfeb024a', 'investigative_problem_solving', null, 'level_two', 'IPS-2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11467, '08473358-8c16-4375-bdeb-d468dfeb024a', 'investigative_problem_solving', null, 'level_two', 'IPS-2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11468, '08473358-8c16-4375-bdeb-d468dfeb024a', 'investigative_problem_solving', null, 'level_two', 'IPS-2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11469, '08473358-8c16-4375-bdeb-d468dfeb024a', 'investigative_problem_solving', null, 'level_two', 'IPS-2.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11470, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_two', 'PL-2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11471, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_two', 'PL-2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11472, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_two', 'PL-2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11473, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_two', 'PL-2.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11474, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_two', 'PL-2.5');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11475, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_two', 'PL-2.6');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11476, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_two', 'TI-2.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11477, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_two', 'TI-2.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11478, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_two', 'TI-2.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11479, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_two', 'TI-2.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11480, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_two', 'TI-2.5');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (11481, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_two', 'TI-2.6');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12286, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_three', 'FOO-3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12287, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_three', 'FOO-3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12288, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_three', 'FOO-3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12289, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_three', 'FOO-3.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12290, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_three', 'FOO-3.5');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12291, '08473358-8c16-4375-bdeb-d468dfeb024a', 'investigative_problem_solving', null, 'level_three', 'IPS-3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12292, '08473358-8c16-4375-bdeb-d468dfeb024a', 'investigative_problem_solving', null, 'level_three', 'IPS-3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12293, '08473358-8c16-4375-bdeb-d468dfeb024a', 'investigative_problem_solving', null, 'level_three', 'IPS-3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12294, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_three', 'PL-3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12295, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_three', 'PL-3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12296, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_three', 'PL-3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12297, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_three', 'PL-3.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12298, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_three', 'PL-3.5');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12299, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_three', 'TI-3.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12300, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_three', 'TI-3.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (12301, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_three', 'TI-3.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22323, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_one', 'FOO-1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22324, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_one', 'FOO-1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22325, '08473358-8c16-4375-bdeb-d468dfeb024a', 'investigative_problem_solving', null, 'level_one', 'IPS-1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22326, '08473358-8c16-4375-bdeb-d468dfeb024a', 'investigative_problem_solving', null, 'level_one', 'IPS-1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22327, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_one', 'PL-1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22328, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_one', 'PL-1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22329, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_one', 'PL-1.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22330, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_one', 'PL-1.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22331, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_one', 'PL-1.5');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22332, '08473358-8c16-4375-bdeb-d468dfeb024a', 'personal_leadership', null, 'level_one', 'PL-1.6');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22333, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_one', 'TI-1.1');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22334, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_one', 'TI-1.2');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22335, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_one', 'TI-1.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22336, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_one', 'TI-1.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22337, '08473358-8c16-4375-bdeb-d468dfeb024a', 'targeted_interaction', null, 'level_one', 'TI-1.5');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22338, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_one', 'FOO-1.3');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22339, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_one', 'FOO-1.4');
INSERT INTO epsilon.LearningDomainOutcomes (Id, TenantId, RowId, ColumnId, ValueId, Name) VALUES (22340, '08473358-8c16-4375-bdeb-d468dfeb024a', 'future_oriented_organisation', null, 'level_one', 'FOO-1.5');
INSERT INTO epsilon.LearningDomains (Id, RowsSetId, ColumnsSetId, ValuesSetId) VALUES ('hbo-i-2018', '87c43afe-05df-460d-96b7-826575c38a1b', '2ad0d764-229b-4248-8667-ea8405b557d5', '7d06d502-6f1f-45b8-b9c5-4521c2260433');
INSERT INTO epsilon.LearningDomains (Id, RowsSetId, ColumnsSetId, ValuesSetId) VALUES ('pd-2020-bsc', '8c349a40-2bec-49af-b2bf-6a3a19f44d34', null, '7d06d502-6f1f-45b8-b9c5-4521c2260433');
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('advise', 'Advise', 'Adv', null);
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('analysis', 'Analysis', 'Ana', null);
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('design', 'Design', 'Des', null);
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('future_oriented_organisation', 'Future-Oriented Organisation', 'FOO', null);
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('hardware_interfacing', 'Hardware Interfacing', 'H', '8D9292');
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('infrastructure', 'Infrastructure', 'I', '6EA7D4');
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('investigative_problem_solving', 'Investigative Problem Solving', 'IPS', null);
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('level_four', 'Level 4', '4', 'B15EB2');
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('level_one', 'Level 1', '1', '8EAADB');
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('level_three', 'Level 3', '3', 'FFD965');
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('level_two', 'Level 2', '2', 'A8D08D');
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('manage_control', 'Manage & Control', 'M&C', null);
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('organisational_processes', 'Organisational Processes', 'O', 'D16557');
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('personal_leadership', 'Personal Leadership', 'PL', null);
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('realisation', 'Realisation', 'Rea', null);
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('software', 'Software', 'S', '96B9C0');
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('targeted_interaction', 'Targeted Interaction', 'TI', null);
INSERT INTO epsilon.LearningDomainTypes (Id, Name, ShortName, HexColor) VALUES ('user_interaction', 'User Interaction', 'U', 'E29C53');
INSERT INTO epsilon.LearningDomainTypeSet (Id) VALUES ('2ad0d764-229b-4248-8667-ea8405b557d5');
INSERT INTO epsilon.LearningDomainTypeSet (Id) VALUES ('7d06d502-6f1f-45b8-b9c5-4521c2260433');
INSERT INTO epsilon.LearningDomainTypeSet (Id) VALUES ('87c43afe-05df-460d-96b7-826575c38a1b');
INSERT INTO epsilon.LearningDomainTypeSet (Id) VALUES ('8c349a40-2bec-49af-b2bf-6a3a19f44d34');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('2ad0d764-229b-4248-8667-ea8405b557d5', 'advise');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('2ad0d764-229b-4248-8667-ea8405b557d5', 'analysis');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('2ad0d764-229b-4248-8667-ea8405b557d5', 'design');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('2ad0d764-229b-4248-8667-ea8405b557d5', 'manage_control');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('2ad0d764-229b-4248-8667-ea8405b557d5', 'realisation');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('7d06d502-6f1f-45b8-b9c5-4521c2260433', 'level_four');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('7d06d502-6f1f-45b8-b9c5-4521c2260433', 'level_one');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('7d06d502-6f1f-45b8-b9c5-4521c2260433', 'level_three');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('7d06d502-6f1f-45b8-b9c5-4521c2260433', 'level_two');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('87c43afe-05df-460d-96b7-826575c38a1b', 'hardware_interfacing');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('87c43afe-05df-460d-96b7-826575c38a1b', 'infrastructure');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('87c43afe-05df-460d-96b7-826575c38a1b', 'organisational_processes');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('87c43afe-05df-460d-96b7-826575c38a1b', 'software');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('87c43afe-05df-460d-96b7-826575c38a1b', 'user_interaction');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('8c349a40-2bec-49af-b2bf-6a3a19f44d34', 'future_oriented_organisation');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('8c349a40-2bec-49af-b2bf-6a3a19f44d34', 'investigative_problem_solving');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('8c349a40-2bec-49af-b2bf-6a3a19f44d34', 'personal_leadership');
INSERT INTO epsilon.LearningDomainTypeSetTypes (SetsId, TypesId) VALUES ('8c349a40-2bec-49af-b2bf-6a3a19f44d34', 'targeted_interaction');