using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLParser.Classes {
    internal class Visitor : TSqlConcreteFragmentVisitor {
        public readonly List<TSqlFragment> Statements = new List<TSqlFragment>();

        public override void Visit(SelectStatement node) {
            Statements.Add(node);
        }
        public override void Visit(InsertStatement node) {
            Statements.Add(node);
        }
        public override void Visit(UpdateStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DeleteStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateTableStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateViewStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableAddTableElementStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableAlterColumnStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableAlterIndexStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableAlterPartitionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableChangeTrackingModificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableConstraintModificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableDropTableElement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableDropTableElementStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableFileTableNamespaceStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableRebuildStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableSwitchStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableSetStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTableTriggerModificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterColumnAlterFullTextIndexAction node) {
            Statements.Add(node);
        }
        public override void Visit(AlterColumnEncryptionKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseAddFileGroupStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseAddFileStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseAuditSpecificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseCollateStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseEncryptionKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseModifyFileGroupStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseModifyFileStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseModifyNameStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseRebuildLogStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseRemoveFileGroupStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseRemoveFileStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseScopedConfigurationClearStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseScopedConfigurationSetStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseSetStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterDatabaseTermination node) {
            Statements.Add(node);
        }
        public override void Visit(AcceleratedDatabaseRecoveryDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(AddAlterFullTextIndexAction node) {
            Statements.Add(node);
        }
        public override void Visit(AddFileSpec node) {
            Statements.Add(node);
        }
        public override void Visit(AddMemberAlterRoleAction node) {
            Statements.Add(node);
        }
        public override void Visit(AddSearchPropertyListAction node) {
            Statements.Add(node);
        }
        public override void Visit(AddSensitivityClassificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AddSignatureStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AdHocDataSource node) {
            Statements.Add(node);
        }
        public override void Visit(AdHocTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(AlgorithmKeyOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterApplicationRoleStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterAssemblyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterAsymmetricKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterAuthorizationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterAvailabilityGroupAction node) {
            Statements.Add(node);
        }
        public override void Visit(AlterAvailabilityGroupFailoverAction node) {
            Statements.Add(node);
        }
        public override void Visit(AlterAvailabilityGroupFailoverOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterAvailabilityGroupStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterBrokerPriorityStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterCertificateStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterCredentialStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterCryptographicProviderStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterEndpointStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterEventSessionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterExternalDataSourceStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterExternalResourcePoolStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterFederationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterFullTextCatalogStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterFullTextIndexStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterFullTextStopListStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterFunctionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterIndexStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterLoginAddDropCredentialStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterLoginEnableDisableStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterLoginOptionsStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterMessageTypeStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterPartitionFunctionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterPartitionSchemeStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterProcedureStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterQueueStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterRemoteServiceBindingStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterResourceGovernorStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterResourcePoolStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterRoleStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterRouteStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterSchemaStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterSearchPropertyListStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterSecurityPolicyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterSequenceStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerAuditSpecificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerAuditStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationBufferPoolExtensionContainerOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationBufferPoolExtensionOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationBufferPoolExtensionSizeOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationDiagnosticsLogMaxSizeOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationDiagnosticsLogOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationExternalAuthenticationContainerOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationExternalAuthenticationOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationFailoverClusterPropertyOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationHadrClusterOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationSetBufferPoolExtensionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationSetDiagnosticsLogStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationSetExternalAuthenticationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationSetFailoverClusterPropertyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationSetHadrClusterStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationSetSoftNumaStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationSoftNumaOption node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerConfigurationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServerRoleStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServiceMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterServiceStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterSymmetricKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterTriggerStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterUserStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterViewStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterWorkloadGroupStatement node) {
            Statements.Add(node);
        }
        public override void Visit(AlterXmlSchemaCollectionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ApplicationRoleOption node) {
            Statements.Add(node);
        }
        public override void Visit(AssemblyEncryptionSource node) {
            Statements.Add(node);
        }
        public override void Visit(AssemblyName node) {
            Statements.Add(node);
        }
        public override void Visit(AssemblyOption node) {
            Statements.Add(node);
        }
        public override void Visit(AssignmentSetClause node) {
            Statements.Add(node);
        }
        public override void Visit(AsymmetricKeyCreateLoginSource node) {
            Statements.Add(node);
        }
        public override void Visit(AtTimeZoneCall node) {
            Statements.Add(node);
        }
        public override void Visit(AuditActionGroupReference node) {
            Statements.Add(node);
        }
        public override void Visit(AuditActionSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(AuditGuidAuditOption node) {
            Statements.Add(node);
        }
        public override void Visit(AuditSpecificationPart node) {
            Statements.Add(node);
        }
        public override void Visit(AuditTarget node) {
            Statements.Add(node);
        }
        public override void Visit(AuthenticationEndpointProtocolOption node) {
            Statements.Add(node);
        }
        public override void Visit(AuthenticationPayloadOption node) {
            Statements.Add(node);
        }
        public override void Visit(AutoCleanupChangeTrackingOptionDetail node) {
            Statements.Add(node);
        }
        public override void Visit(AutoCreateStatisticsDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(AutomaticTuningCreateIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(AutomaticTuningDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(AutomaticTuningDropIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(AutomaticTuningForceLastGoodPlanOption node) {
            Statements.Add(node);
        }
        public override void Visit(AutomaticTuningMaintainIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(AutomaticTuningOption node) {
            Statements.Add(node);
        }
        public override void Visit(AvailabilityModeReplicaOption node) {
            Statements.Add(node);
        }
        public override void Visit(AvailabilityReplica node) {
            Statements.Add(node);
        }
        public override void Visit(BackupCertificateStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BackupDatabaseStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BackupEncryptionOption node) {
            Statements.Add(node);
        }
        public override void Visit(BackupMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BackupOption node) {
            Statements.Add(node);
        }
        public override void Visit(BackupRestoreFileInfo node) {
            Statements.Add(node);
        }
        public override void Visit(BackupServiceMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BackupTransactionLogStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BackwardsCompatibleDropIndexClause node) {
            Statements.Add(node);
        }
        public override void Visit(BeginConversationTimerStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BeginDialogStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BeginEndAtomicBlockStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BeginEndBlockStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BeginTransactionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BinaryExpression node) {
            Statements.Add(node);
        }
        public override void Visit(BinaryLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(BinaryQueryExpression node) {
            Statements.Add(node);
        }
        public override void Visit(BooleanBinaryExpression node) {
            Statements.Add(node);
        }
        public override void Visit(BooleanComparisonExpression node) {
            Statements.Add(node);
        }
        public override void Visit(BooleanExpressionSnippet node) {
            Statements.Add(node);
        }
        public override void Visit(BooleanIsNullExpression node) {
            Statements.Add(node);
        }
        public override void Visit(BooleanNotExpression node) {
            Statements.Add(node);
        }
        public override void Visit(BooleanParenthesisExpression node) {
            Statements.Add(node);
        }
        public override void Visit(BooleanTernaryExpression node) {
            Statements.Add(node);
        }
        public override void Visit(BoundingBoxParameter node) {
            Statements.Add(node);
        }
        public override void Visit(BoundingBoxSpatialIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(BreakStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BrokerPriorityParameter node) {
            Statements.Add(node);
        }
        public override void Visit(BrowseForClause node) {
            Statements.Add(node);
        }
        public override void Visit(BuiltInFunctionTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(BulkInsertOption node) {
            Statements.Add(node);
        }
        public override void Visit(BulkInsertStatement node) {
            Statements.Add(node);
        }
        public override void Visit(BulkOpenRowset node) {
            Statements.Add(node);
        }
        public override void Visit(CastCall node) {
            Statements.Add(node);
        }
        public override void Visit(CatalogCollationOption node) {
            Statements.Add(node);
        }
        public override void Visit(CellsPerObjectSpatialIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(CertificateCreateLoginSource node) {
            Statements.Add(node);
        }
        public override void Visit(CertificateOption node) {
            Statements.Add(node);
        }
        public override void Visit(ChangeRetentionChangeTrackingOptionDetail node) {
            Statements.Add(node);
        }
        public override void Visit(ChangeTableChangesTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(ChangeTableVersionTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(ChangeTrackingDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(ChangeTrackingFullTextIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(CharacterSetPayloadOption node) {
            Statements.Add(node);
        }
        public override void Visit(CheckConstraintDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(CheckpointStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ChildObjectName node) {
            Statements.Add(node);
        }
        public override void Visit(ClassifierEndTimeOption node) {
            Statements.Add(node);
        }
        public override void Visit(ClassifierImportanceOption node) {
            Statements.Add(node);
        }
        public override void Visit(ClassifierMemberNameOption node) {
            Statements.Add(node);
        }
        public override void Visit(ClassifierStartTimeOption node) {
            Statements.Add(node);
        }
        public override void Visit(ClassifierWlmContextOption node) {
            Statements.Add(node);
        }
        public override void Visit(ClassifierWlmLabelOption node) {
            Statements.Add(node);
        }
        public override void Visit(ClassifierWorkloadGroupOption node) {
            Statements.Add(node);
        }
        public override void Visit(CloseCursorStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CloseMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CloseSymmetricKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CoalesceExpression node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnDefinitionBase node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnEncryptionAlgorithmNameParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnEncryptionAlgorithmParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnEncryptionDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnEncryptionKeyNameParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnEncryptionKeyValue node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnEncryptionTypeParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnMasterKeyEnclaveComputationsParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnMasterKeyNameParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnMasterKeyPathParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnMasterKeyStoreProviderNameParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnReferenceExpression node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnStorageOptions node) {
            Statements.Add(node);
        }
        public override void Visit(ColumnWithSortOrder node) {
            Statements.Add(node);
        }
        public override void Visit(CommandSecurityElement80 node) {
            Statements.Add(node);
        }
        public override void Visit(CommitTransactionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CommonTableExpression node) {
            Statements.Add(node);
        }
        public override void Visit(CompositeGroupingSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(CompressionDelayIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(CompressionEndpointProtocolOption node) {
            Statements.Add(node);
        }
        public override void Visit(CompressionPartitionRange node) {
            Statements.Add(node);
        }
        public override void Visit(ComputeClause node) {
            Statements.Add(node);
        }
        public override void Visit(ComputeFunction node) {
            Statements.Add(node);
        }
        public override void Visit(ContainmentDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(ContinueStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ContractMessage node) {
            Statements.Add(node);
        }
        public override void Visit(ConvertCall node) {
            Statements.Add(node);
        }
        public override void Visit(CopyColumnOption node) {
            Statements.Add(node);
        }
        public override void Visit(CopyCredentialOption node) {
            Statements.Add(node);
        }
        public override void Visit(CopyOption node) {
            Statements.Add(node);
        }
        public override void Visit(CopyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateAggregateStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateApplicationRoleStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateAssemblyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateAsymmetricKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateAvailabilityGroupStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateBrokerPriorityStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateCertificateStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateColumnEncryptionKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateColumnMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateColumnStoreIndexStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateContractStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateCredentialStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateCryptographicProviderStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateDatabaseAuditSpecificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateDatabaseEncryptionKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateDatabaseStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateDefaultStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateEndpointStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateEventNotificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateEventSessionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateExternalDataSourceStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateExternalFileFormatStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateExternalResourcePoolStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateExternalTableStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateFederationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateFullTextCatalogStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateFullTextIndexStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateFullTextStopListStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateFunctionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateIndexStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateLoginStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateMessageTypeStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateOrAlterFunctionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateOrAlterProcedureStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateOrAlterTriggerStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateOrAlterViewStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreatePartitionFunctionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreatePartitionSchemeStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateProcedureStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateQueueStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateRemoteServiceBindingStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateResourcePoolStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateRoleStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateRouteStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateRuleStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateSchemaStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateSearchPropertyListStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateSecurityPolicyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateSelectiveXmlIndexStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateSequenceStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateServerAuditSpecificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateServerAuditStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateServerRoleStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateServiceStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateSpatialIndexStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateStatisticsStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateSymmetricKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateSynonymStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateTriggerStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateTypeTableStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateTypeUddtStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateTypeUdtStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateUserStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateWorkloadClassifierStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateWorkloadGroupStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateXmlIndexStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreateXmlSchemaCollectionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(CreationDispositionKeyOption node) {
            Statements.Add(node);
        }
        public override void Visit(CryptoMechanism node) {
            Statements.Add(node);
        }
        public override void Visit(CubeGroupingSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(CursorDefaultDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(CursorDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(CursorId node) {
            Statements.Add(node);
        }
        public override void Visit(CursorOption node) {
            Statements.Add(node);
        }
        public override void Visit(DatabaseAuditAction node) {
            Statements.Add(node);
        }
        public override void Visit(DatabaseConfigurationClearOption node) {
            Statements.Add(node);
        }
        public override void Visit(DatabaseConfigurationSetOption node) {
            Statements.Add(node);
        }
        public override void Visit(DatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(DataCompressionOption node) {
            Statements.Add(node);
        }
        public override void Visit(DataModificationTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(DataTypeSequenceOption node) {
            Statements.Add(node);
        }
        public override void Visit(DbccNamedLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(DbccOption node) {
            Statements.Add(node);
        }
        public override void Visit(DbccStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DeallocateCursorStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DeclareCursorStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DeclareTableVariableBody node) {
            Statements.Add(node);
        }
        public override void Visit(DeclareTableVariableStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DeclareVariableElement node) {
            Statements.Add(node);
        }
        public override void Visit(DeclareVariableStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DefaultConstraintDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(DefaultLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(DelayedDurabilityDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(DeleteMergeAction node) {
            Statements.Add(node);
        }
        public override void Visit(DeleteSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(DenyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DenyStatement80 node) {
            Statements.Add(node);
        }
        public override void Visit(DeviceInfo node) {
            Statements.Add(node);
        }
        public override void Visit(DiskStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DiskStatementOption node) {
            Statements.Add(node);
        }
        public override void Visit(DropAggregateStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropAlterFullTextIndexAction node) {
            Statements.Add(node);
        }
        public override void Visit(DropApplicationRoleStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropAssemblyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropAsymmetricKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropAvailabilityGroupStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropBrokerPriorityStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropCertificateStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropClusteredConstraintMoveOption node) {
            Statements.Add(node);
        }
        public override void Visit(DropClusteredConstraintStateOption node) {
            Statements.Add(node);
        }
        public override void Visit(DropClusteredConstraintValueOption node) {
            Statements.Add(node);
        }
        public override void Visit(DropClusteredConstraintWaitAtLowPriorityLockOption node) {
            Statements.Add(node);
        }
        public override void Visit(DropColumnEncryptionKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropColumnMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropContractStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropCredentialStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropCryptographicProviderStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropDatabaseAuditSpecificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropDatabaseEncryptionKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropDatabaseStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropDefaultStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropEndpointStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropEventNotificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropEventSessionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropExternalDataSourceStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropExternalFileFormatStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropExternalResourcePoolStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropExternalTableStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropFederationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropFullTextCatalogStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropFullTextIndexStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropFullTextStopListStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropFunctionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropIndexClause node) {
            Statements.Add(node);
        }
        public override void Visit(DropIndexStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropLoginStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropMemberAlterRoleAction node) {
            Statements.Add(node);
        }
        public override void Visit(DropMessageTypeStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropPartitionFunctionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropPartitionSchemeStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropProcedureStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropQueueStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropRemoteServiceBindingStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropResourcePoolStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropRoleStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropRouteStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropRuleStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropSchemaStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropSearchPropertyListAction node) {
            Statements.Add(node);
        }
        public override void Visit(DropSearchPropertyListStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropSecurityPolicyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropSensitivityClassificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropSequenceStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropServerAuditSpecificationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropServerAuditStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropServerRoleStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropServiceStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropSignatureStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropStatisticsStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropSymmetricKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropSynonymStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropTableStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropTriggerStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropTypeStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropUserStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropViewStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropWorkloadClassifierStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropWorkloadGroupStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DropXmlSchemaCollectionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(DurabilityTableOption node) {
            Statements.Add(node);
        }
        public override void Visit(EnabledDisabledPayloadOption node) {
            Statements.Add(node);
        }
        public override void Visit(EnableDisableTriggerStatement node) {
            Statements.Add(node);
        }
        public override void Visit(EncryptedValueParameter node) {
            Statements.Add(node);
        }
        public override void Visit(EncryptionPayloadOption node) {
            Statements.Add(node);
        }
        public override void Visit(EndConversationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(EndpointAffinity node) {
            Statements.Add(node);
        }
        public override void Visit(EventDeclaration node) {
            Statements.Add(node);
        }
        public override void Visit(EventDeclarationCompareFunctionParameter node) {
            Statements.Add(node);
        }
        public override void Visit(EventDeclarationSetParameter node) {
            Statements.Add(node);
        }
        public override void Visit(EventGroupContainer node) {
            Statements.Add(node);
        }
        public override void Visit(EventNotificationObjectScope node) {
            Statements.Add(node);
        }
        public override void Visit(EventRetentionSessionOption node) {
            Statements.Add(node);
        }
        public override void Visit(EventSessionObjectName node) {
            Statements.Add(node);
        }
        public override void Visit(EventSessionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(EventTypeContainer node) {
            Statements.Add(node);
        }
        public override void Visit(ExecutableProcedureReference node) {
            Statements.Add(node);
        }
        public override void Visit(ExecutableStringList node) {
            Statements.Add(node);
        }
        public override void Visit(ExecuteAsClause node) {
            Statements.Add(node);
        }
        public override void Visit(ExecuteAsFunctionOption node) {
            Statements.Add(node);
        }
        public override void Visit(ExecuteAsProcedureOption node) {
            Statements.Add(node);
        }
        public override void Visit(ExecuteAsStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ExecuteAsTriggerOption node) {
            Statements.Add(node);
        }
        public override void Visit(ExecuteContext node) {
            Statements.Add(node);
        }
        public override void Visit(ExecuteInsertSource node) {
            Statements.Add(node);
        }
        public override void Visit(ExecuteOption node) {
            Statements.Add(node);
        }
        public override void Visit(ExecuteParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ExecuteSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(ExecuteStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ExistsPredicate node) {
            Statements.Add(node);
        }
        public override void Visit(ExpressionCallTarget node) {
            Statements.Add(node);
        }
        public override void Visit(ExpressionGroupingSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(ExpressionWithSortOrder node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalCreateLoginSource node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalDataSourceLiteralOrIdentifierOption node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalFileFormatContainerOption node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalFileFormatLiteralOption node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalFileFormatUseDefaultTypeOption node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalResourcePoolAffinitySpecification node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalResourcePoolParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalResourcePoolStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalTableColumnDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalTableDistributionOption node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalTableLiteralOrIdentifierOption node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalTableRejectTypeOption node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalTableReplicatedDistributionPolicy node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalTableRoundRobinDistributionPolicy node) {
            Statements.Add(node);
        }
        public override void Visit(ExternalTableShardedDistributionPolicy node) {
            Statements.Add(node);
        }
        public override void Visit(ExtractFromExpression node) {
            Statements.Add(node);
        }
        public override void Visit(FailoverModeReplicaOption node) {
            Statements.Add(node);
        }
        public override void Visit(FederationScheme node) {
            Statements.Add(node);
        }
        public override void Visit(FetchCursorStatement node) {
            Statements.Add(node);
        }
        public override void Visit(FetchType node) {
            Statements.Add(node);
        }
        public override void Visit(FileDeclaration node) {
            Statements.Add(node);
        }
        public override void Visit(FileDeclarationOption node) {
            Statements.Add(node);
        }
        public override void Visit(FileEncryptionSource node) {
            Statements.Add(node);
        }
        public override void Visit(FileGroupDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(FileGroupOrPartitionScheme node) {
            Statements.Add(node);
        }
        public override void Visit(FileGrowthFileDeclarationOption node) {
            Statements.Add(node);
        }
        public override void Visit(FileNameFileDeclarationOption node) {
            Statements.Add(node);
        }
        public override void Visit(FileStreamDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(FileStreamOnDropIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(FileStreamOnTableOption node) {
            Statements.Add(node);
        }
        public override void Visit(FileStreamRestoreOption node) {
            Statements.Add(node);
        }
        public override void Visit(FileTableCollateFileNameTableOption node) {
            Statements.Add(node);
        }
        public override void Visit(FileTableConstraintNameTableOption node) {
            Statements.Add(node);
        }
        public override void Visit(FileTableDirectoryTableOption node) {
            Statements.Add(node);
        }
        public override void Visit(ForceSeekTableHint node) {
            Statements.Add(node);
        }
        public override void Visit(ForeignKeyConstraintDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(FromClause node) {
            Statements.Add(node);
        }
        public override void Visit(FullTextCatalogAndFileGroup node) {
            Statements.Add(node);
        }
        public override void Visit(FullTextIndexColumn node) {
            Statements.Add(node);
        }
        public override void Visit(FullTextPredicate node) {
            Statements.Add(node);
        }
        public override void Visit(FullTextStopListAction node) {
            Statements.Add(node);
        }
        public override void Visit(FullTextTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(FunctionCall node) {
            Statements.Add(node);
        }
        public override void Visit(FunctionCallSetClause node) {
            Statements.Add(node);
        }
        public override void Visit(FunctionOption node) {
            Statements.Add(node);
        }
        public override void Visit(GeneralSetCommand node) {
            Statements.Add(node);
        }
        public override void Visit(GenericConfigurationOption node) {
            Statements.Add(node);
        }
        public override void Visit(GetConversationGroupStatement node) {
            Statements.Add(node);
        }
        public override void Visit(GlobalFunctionTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(GlobalVariableExpression node) {
            Statements.Add(node);
        }
        public override void Visit(GoToStatement node) {
            Statements.Add(node);
        }
        public override void Visit(GrandTotalGroupingSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(GrantStatement node) {
            Statements.Add(node);
        }
        public override void Visit(GrantStatement80 node) {
            Statements.Add(node);
        }
        public override void Visit(GraphConnectionBetweenNodes node) {
            Statements.Add(node);
        }
        public override void Visit(GraphConnectionConstraintDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(GraphMatchCompositeExpression node) {
            Statements.Add(node);
        }
        public override void Visit(GraphMatchExpression node) {
            Statements.Add(node);
        }
        public override void Visit(GraphMatchLastNodePredicate node) {
            Statements.Add(node);
        }
        public override void Visit(GraphMatchNodeExpression node) {
            Statements.Add(node);
        }
        public override void Visit(GraphMatchPredicate node) {
            Statements.Add(node);
        }
        public override void Visit(GraphMatchRecursivePredicate node) {
            Statements.Add(node);
        }
        public override void Visit(GraphRecursiveMatchQuantifier node) {
            Statements.Add(node);
        }
        public override void Visit(GridParameter node) {
            Statements.Add(node);
        }
        public override void Visit(GridsSpatialIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(GroupByClause node) {
            Statements.Add(node);
        }
        public override void Visit(GroupingSetsGroupingSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(HadrAvailabilityGroupDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(HadrDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(HavingClause node) {
            Statements.Add(node);
        }
        public override void Visit(Identifier node) {
            Statements.Add(node);
        }
        public override void Visit(IdentifierAtomicBlockOption node) {
            Statements.Add(node);
        }
        public override void Visit(IdentifierDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(IdentifierLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(IdentifierOrScalarExpression node) {
            Statements.Add(node);
        }
        public override void Visit(IdentifierOrValueExpression node) {
            Statements.Add(node);
        }
        public override void Visit(IdentifierPrincipalOption node) {
            Statements.Add(node);
        }
        public override void Visit(IdentifierSnippet node) {
            Statements.Add(node);
        }
        public override void Visit(IdentityFunctionCall node) {
            Statements.Add(node);
        }
        public override void Visit(IdentityOptions node) {
            Statements.Add(node);
        }
        public override void Visit(IdentityValueKeyOption node) {
            Statements.Add(node);
        }
        public override void Visit(IfStatement node) {
            Statements.Add(node);
        }
        public override void Visit(IgnoreDupKeyIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(IIfCall node) {
            Statements.Add(node);
        }
        public override void Visit(IndexDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(IndexExpressionOption node) {
            Statements.Add(node);
        }
        public override void Visit(IndexStateOption node) {
            Statements.Add(node);
        }
        public override void Visit(IndexTableHint node) {
            Statements.Add(node);
        }
        public override void Visit(IndexType node) {
            Statements.Add(node);
        }
        public override void Visit(InlineDerivedTable node) {
            Statements.Add(node);
        }
        public override void Visit(InlineFunctionOption node) {
            Statements.Add(node);
        }
        public override void Visit(InlineResultSetDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(InPredicate node) {
            Statements.Add(node);
        }
        public override void Visit(InsertBulkColumnDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(InsertBulkStatement node) {
            Statements.Add(node);
        }
        public override void Visit(InsertMergeAction node) {
            Statements.Add(node);
        }
        public override void Visit(InsertSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(IntegerLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(InternalOpenRowset node) {
            Statements.Add(node);
        }
        public override void Visit(IPv4 node) {
            Statements.Add(node);
        }
        public override void Visit(JoinParenthesisTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(JsonForClause node) {
            Statements.Add(node);
        }
        public override void Visit(JsonForClauseOption node) {
            Statements.Add(node);
        }
        public override void Visit(KeySourceKeyOption node) {
            Statements.Add(node);
        }
        public override void Visit(KillQueryNotificationSubscriptionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(KillStatement node) {
            Statements.Add(node);
        }
        public override void Visit(KillStatsJobStatement node) {
            Statements.Add(node);
        }
        public override void Visit(LabelStatement node) {
            Statements.Add(node);
        }
        public override void Visit(LeftFunctionCall node) {
            Statements.Add(node);
        }
        public override void Visit(LikePredicate node) {
            Statements.Add(node);
        }
        public override void Visit(LineNoStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ListenerIPEndpointProtocolOption node) {
            Statements.Add(node);
        }
        public override void Visit(ListTypeCopyOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralAtomicBlockOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralAuditTargetOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralAvailabilityGroupOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralBulkInsertOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralEndpointProtocolOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralOptimizerHint node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralOptionValue node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralPayloadOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralPrincipalOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralRange node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralReplicaOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralSessionOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralStatisticsOption node) {
            Statements.Add(node);
        }
        public override void Visit(LiteralTableHint node) {
            Statements.Add(node);
        }
        public override void Visit(LocationOption node) {
            Statements.Add(node);
        }
        public override void Visit(LockEscalationTableOption node) {
            Statements.Add(node);
        }
        public override void Visit(LoginTypePayloadOption node) {
            Statements.Add(node);
        }
        public override void Visit(LowPriorityLockWaitAbortAfterWaitOption node) {
            Statements.Add(node);
        }
        public override void Visit(LowPriorityLockWaitMaxDurationOption node) {
            Statements.Add(node);
        }
        public override void Visit(LowPriorityLockWaitTableSwitchOption node) {
            Statements.Add(node);
        }
        public override void Visit(MaxDispatchLatencySessionOption node) {
            Statements.Add(node);
        }
        public override void Visit(MaxDopConfigurationOption node) {
            Statements.Add(node);
        }
        public override void Visit(MaxDurationOption node) {
            Statements.Add(node);
        }
        public override void Visit(MaxLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(MaxRolloverFilesAuditTargetOption node) {
            Statements.Add(node);
        }
        public override void Visit(MaxSizeAuditTargetOption node) {
            Statements.Add(node);
        }
        public override void Visit(MaxSizeDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(MaxSizeFileDeclarationOption node) {
            Statements.Add(node);
        }
        public override void Visit(MemoryOptimizedTableOption node) {
            Statements.Add(node);
        }
        public override void Visit(MemoryPartitionSessionOption node) {
            Statements.Add(node);
        }
        public override void Visit(MergeActionClause node) {
            Statements.Add(node);
        }
        public override void Visit(MergeSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(MergeStatement node) {
            Statements.Add(node);
        }
        public override void Visit(MethodSpecifier node) {
            Statements.Add(node);
        }
        public override void Visit(MirrorToClause node) {
            Statements.Add(node);
        }
        public override void Visit(MoneyLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(MoveConversationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(MoveRestoreOption node) {
            Statements.Add(node);
        }
        public override void Visit(MoveToDropIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(MultiPartIdentifier node) {
            Statements.Add(node);
        }
        public override void Visit(MultiPartIdentifierCallTarget node) {
            Statements.Add(node);
        }
        public override void Visit(NamedTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(NameFileDeclarationOption node) {
            Statements.Add(node);
        }
        public override void Visit(NextValueForExpression node) {
            Statements.Add(node);
        }
        public override void Visit(NullableConstraintDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(NullIfExpression node) {
            Statements.Add(node);
        }
        public override void Visit(NullLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(NumericLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(OdbcConvertSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(OdbcFunctionCall node) {
            Statements.Add(node);
        }
        public override void Visit(OdbcLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(OdbcQualifiedJoinTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(OffsetClause node) {
            Statements.Add(node);
        }
        public override void Visit(OnFailureAuditOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnlineIndexLowPriorityLockWaitOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnlineIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffAssemblyOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffAtomicBlockOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffAuditTargetOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffDialogOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffFullTextCatalogOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffOptionValue node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffPrimaryConfigurationOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffPrincipalOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffRemoteServiceBindingOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffSessionOption node) {
            Statements.Add(node);
        }
        public override void Visit(OnOffStatisticsOption node) {
            Statements.Add(node);
        }
        public override void Visit(OpenCursorStatement node) {
            Statements.Add(node);
        }
        public override void Visit(OpenJsonTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(OpenMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(OpenQueryTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(OpenRowsetTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(OpenSymmetricKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(OpenXmlTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(OptimizeForOptimizerHint node) {
            Statements.Add(node);
        }
        public override void Visit(OptimizerHint node) {
            Statements.Add(node);
        }
        public override void Visit(OrderBulkInsertOption node) {
            Statements.Add(node);
        }
        public override void Visit(OrderByClause node) {
            Statements.Add(node);
        }
        public override void Visit(OrderIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(OutputClause node) {
            Statements.Add(node);
        }
        public override void Visit(OutputIntoClause node) {
            Statements.Add(node);
        }
        public override void Visit(OverClause node) {
            Statements.Add(node);
        }
        public override void Visit(PageVerifyDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(ParameterizationDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(ParameterlessCall node) {
            Statements.Add(node);
        }
        public override void Visit(ParenthesisExpression node) {
            Statements.Add(node);
        }
        public override void Visit(ParseCall node) {
            Statements.Add(node);
        }
        public override void Visit(PartitionFunctionCall node) {
            Statements.Add(node);
        }
        public override void Visit(PartitionParameterType node) {
            Statements.Add(node);
        }
        public override void Visit(PartitionSpecifier node) {
            Statements.Add(node);
        }
        public override void Visit(PartnerDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(PasswordAlterPrincipalOption node) {
            Statements.Add(node);
        }
        public override void Visit(PasswordCreateLoginSource node) {
            Statements.Add(node);
        }
        public override void Visit(Permission node) {
            Statements.Add(node);
        }
        public override void Visit(PermissionSetAssemblyOption node) {
            Statements.Add(node);
        }
        public override void Visit(PivotedTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(PortsEndpointProtocolOption node) {
            Statements.Add(node);
        }
        public override void Visit(PredicateSetStatement node) {
            Statements.Add(node);
        }
        public override void Visit(PrimaryRoleReplicaOption node) {
            Statements.Add(node);
        }
        public override void Visit(PrincipalOption node) {
            Statements.Add(node);
        }
        public override void Visit(PrintStatement node) {
            Statements.Add(node);
        }
        public override void Visit(Privilege80 node) {
            Statements.Add(node);
        }
        public override void Visit(PrivilegeSecurityElement80 node) {
            Statements.Add(node);
        }
        public override void Visit(ProcedureOption node) {
            Statements.Add(node);
        }
        public override void Visit(ProcedureParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ProcedureReference node) {
            Statements.Add(node);
        }
        public override void Visit(ProcedureReferenceName node) {
            Statements.Add(node);
        }
        public override void Visit(ProcessAffinityRange node) {
            Statements.Add(node);
        }
        public override void Visit(ProviderEncryptionSource node) {
            Statements.Add(node);
        }
        public override void Visit(ProviderKeyNameKeyOption node) {
            Statements.Add(node);
        }
        public override void Visit(QualifiedJoin node) {
            Statements.Add(node);
        }
        public override void Visit(QueryDerivedTable node) {
            Statements.Add(node);
        }
        public override void Visit(QueryParenthesisExpression node) {
            Statements.Add(node);
        }
        public override void Visit(QuerySpecification node) {
            Statements.Add(node);
        }
        public override void Visit(QueryStoreCapturePolicyOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueryStoreDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueryStoreDataFlushIntervalOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueryStoreDesiredStateOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueryStoreIntervalLengthOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueryStoreMaxPlansPerQueryOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueryStoreMaxStorageSizeOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueryStoreSizeCleanupPolicyOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueryStoreTimeCleanupPolicyOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueueDelayAuditOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueueExecuteAsOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueueOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueueProcedureOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueueStateOption node) {
            Statements.Add(node);
        }
        public override void Visit(QueueValueOption node) {
            Statements.Add(node);
        }
        public override void Visit(RaiseErrorLegacyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(RaiseErrorStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ReadOnlyForClause node) {
            Statements.Add(node);
        }
        public override void Visit(ReadTextStatement node) {
            Statements.Add(node);
        }
        public override void Visit(RealLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(ReceiveStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ReconfigureStatement node) {
            Statements.Add(node);
        }
        public override void Visit(RecoveryDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(RemoteDataArchiveAlterTableOption node) {
            Statements.Add(node);
        }
        public override void Visit(RemoteDataArchiveDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(RemoteDataArchiveDbCredentialSetting node) {
            Statements.Add(node);
        }
        public override void Visit(RemoteDataArchiveDbFederatedServiceAccountSetting node) {
            Statements.Add(node);
        }
        public override void Visit(RemoteDataArchiveDbServerSetting node) {
            Statements.Add(node);
        }
        public override void Visit(RemoteDataArchiveTableOption node) {
            Statements.Add(node);
        }
        public override void Visit(RenameAlterRoleAction node) {
            Statements.Add(node);
        }
        public override void Visit(RenameEntityStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ResampleStatisticsOption node) {
            Statements.Add(node);
        }
        public override void Visit(ResourcePoolAffinitySpecification node) {
            Statements.Add(node);
        }
        public override void Visit(ResourcePoolParameter node) {
            Statements.Add(node);
        }
        public override void Visit(ResourcePoolStatement node) {
            Statements.Add(node);
        }
        public override void Visit(RestoreMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(RestoreOption node) {
            Statements.Add(node);
        }
        public override void Visit(RestoreServiceMasterKeyStatement node) {
            Statements.Add(node);
        }
        public override void Visit(RestoreStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ResultColumnDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(ResultSetDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(ResultSetsExecuteOption node) {
            Statements.Add(node);
        }
        public override void Visit(RetentionDaysAuditTargetOption node) {
            Statements.Add(node);
        }
        public override void Visit(RetentionPeriodDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(ReturnStatement node) {
            Statements.Add(node);
        }
        public override void Visit(RevertStatement node) {
            Statements.Add(node);
        }
        public override void Visit(RevokeStatement node) {
            Statements.Add(node);
        }
        public override void Visit(RevokeStatement80 node) {
            Statements.Add(node);
        }
        public override void Visit(RightFunctionCall node) {
            Statements.Add(node);
        }
        public override void Visit(RolePayloadOption node) {
            Statements.Add(node);
        }
        public override void Visit(RollbackTransactionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(RollupGroupingSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(RouteOption node) {
            Statements.Add(node);
        }
        public override void Visit(RowValue node) {
            Statements.Add(node);
        }
        public override void Visit(SaveTransactionStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ScalarExpressionDialogOption node) {
            Statements.Add(node);
        }
        public override void Visit(ScalarExpressionRestoreOption node) {
            Statements.Add(node);
        }
        public override void Visit(ScalarExpressionSequenceOption node) {
            Statements.Add(node);
        }
        public override void Visit(ScalarExpressionSnippet node) {
            Statements.Add(node);
        }
        public override void Visit(ScalarFunctionReturnType node) {
            Statements.Add(node);
        }
        public override void Visit(ScalarSubquery node) {
            Statements.Add(node);
        }
        public override void Visit(SchemaDeclarationItem node) {
            Statements.Add(node);
        }
        public override void Visit(SchemaDeclarationItemOpenjson node) {
            Statements.Add(node);
        }
        public override void Visit(SchemaObjectFunctionTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(SchemaObjectName node) {
            Statements.Add(node);
        }
        public override void Visit(SchemaObjectNameOrValueExpression node) {
            Statements.Add(node);
        }
        public override void Visit(SchemaObjectNameSnippet node) {
            Statements.Add(node);
        }
        public override void Visit(SchemaObjectResultSetDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(SchemaPayloadOption node) {
            Statements.Add(node);
        }
        public override void Visit(SearchedCaseExpression node) {
            Statements.Add(node);
        }
        public override void Visit(SearchedWhenClause node) {
            Statements.Add(node);
        }
        public override void Visit(SearchPropertyListFullTextIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(SecondaryRoleReplicaOption node) {
            Statements.Add(node);
        }
        public override void Visit(SecurityPolicyOption node) {
            Statements.Add(node);
        }
        public override void Visit(SecurityPredicateAction node) {
            Statements.Add(node);
        }
        public override void Visit(SecurityPrincipal node) {
            Statements.Add(node);
        }
        public override void Visit(SecurityTargetObject node) {
            Statements.Add(node);
        }
        public override void Visit(SecurityTargetObjectName node) {
            Statements.Add(node);
        }
        public override void Visit(SecurityUserClause80 node) {
            Statements.Add(node);
        }
        public override void Visit(SelectFunctionReturnType node) {
            Statements.Add(node);
        }
        public override void Visit(SelectInsertSource node) {
            Statements.Add(node);
        }
        public override void Visit(SelectiveXmlIndexPromotedPath node) {
            Statements.Add(node);
        }
        public override void Visit(SelectScalarExpression node) {
            Statements.Add(node);
        }
        public override void Visit(SelectSetVariable node) {
            Statements.Add(node);
        }
        public override void Visit(SelectStarExpression node) {
            Statements.Add(node);
        }
        public override void Visit(SelectStatementSnippet node) {
            Statements.Add(node);
        }
        public override void Visit(SemanticTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(SendStatement node) {
            Statements.Add(node);
        }
        public override void Visit(SensitivityClassificationOption node) {
            Statements.Add(node);
        }
        public override void Visit(SequenceOption node) {
            Statements.Add(node);
        }
        public override void Visit(ServiceContract node) {
            Statements.Add(node);
        }
        public override void Visit(SessionTimeoutPayloadOption node) {
            Statements.Add(node);
        }
        public override void Visit(SetCommandStatement node) {
            Statements.Add(node);
        }
        public override void Visit(SetErrorLevelStatement node) {
            Statements.Add(node);
        }
        public override void Visit(SetFipsFlaggerCommand node) {
            Statements.Add(node);
        }
        public override void Visit(SetIdentityInsertStatement node) {
            Statements.Add(node);
        }
        public override void Visit(SetOffsetsStatement node) {
            Statements.Add(node);
        }
        public override void Visit(SetRowCountStatement node) {
            Statements.Add(node);
        }
        public override void Visit(SetSearchPropertyListAlterFullTextIndexAction node) {
            Statements.Add(node);
        }
        public override void Visit(SetStatisticsStatement node) {
            Statements.Add(node);
        }
        public override void Visit(SetStopListAlterFullTextIndexAction node) {
            Statements.Add(node);
        }
        public override void Visit(SetTextSizeStatement node) {
            Statements.Add(node);
        }
        public override void Visit(SetTransactionIsolationLevelStatement node) {
            Statements.Add(node);
        }
        public override void Visit(SetUserStatement node) {
            Statements.Add(node);
        }
        public override void Visit(SetVariableStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ShutdownStatement node) {
            Statements.Add(node);
        }
        public override void Visit(SimpleAlterFullTextIndexAction node) {
            Statements.Add(node);
        }
        public override void Visit(SimpleCaseExpression node) {
            Statements.Add(node);
        }
        public override void Visit(SimpleWhenClause node) {
            Statements.Add(node);
        }
        public override void Visit(SingleValueTypeCopyOption node) {
            Statements.Add(node);
        }
        public override void Visit(SizeFileDeclarationOption node) {
            Statements.Add(node);
        }
        public override void Visit(SoapMethod node) {
            Statements.Add(node);
        }
        public override void Visit(SourceDeclaration node) {
            Statements.Add(node);
        }
        public override void Visit(SpatialIndexRegularOption node) {
            Statements.Add(node);
        }
        public override void Visit(SqlCommandIdentifier node) {
            Statements.Add(node);
        }
        public override void Visit(SqlDataTypeReference node) {
            Statements.Add(node);
        }
        public override void Visit(StateAuditOption node) {
            Statements.Add(node);
        }
        public override void Visit(StatementList node) {
            Statements.Add(node);
        }
        public override void Visit(StatementListSnippet node) {
            Statements.Add(node);
        }
        public override void Visit(StatisticsOption node) {
            Statements.Add(node);
        }
        public override void Visit(StatisticsPartitionRange node) {
            Statements.Add(node);
        }
        public override void Visit(StopListFullTextIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(StopRestoreOption node) {
            Statements.Add(node);
        }
        public override void Visit(StringLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(SubqueryComparisonPredicate node) {
            Statements.Add(node);
        }
        public override void Visit(SystemTimePeriodDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(SystemVersioningTableOption node) {
            Statements.Add(node);
        }
        public override void Visit(TableClusteredIndexType node) {
            Statements.Add(node);
        }
        public override void Visit(TableDataCompressionOption node) {
            Statements.Add(node);
        }
        public override void Visit(TableDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(TableDistributionOption node) {
            Statements.Add(node);
        }
        public override void Visit(TableHashDistributionPolicy node) {
            Statements.Add(node);
        }
        public override void Visit(TableHint node) {
            Statements.Add(node);
        }
        public override void Visit(TableHintsOptimizerHint node) {
            Statements.Add(node);
        }
        public override void Visit(TableIndexOption node) {
            Statements.Add(node);
        }
        public override void Visit(TableNonClusteredIndexType node) {
            Statements.Add(node);
        }
        public override void Visit(TablePartitionOption node) {
            Statements.Add(node);
        }
        public override void Visit(TablePartitionOptionSpecifications node) {
            Statements.Add(node);
        }
        public override void Visit(TableReplicateDistributionPolicy node) {
            Statements.Add(node);
        }
        public override void Visit(TableRoundRobinDistributionPolicy node) {
            Statements.Add(node);
        }
        public override void Visit(TableSampleClause node) {
            Statements.Add(node);
        }
        public override void Visit(TableValuedFunctionReturnType node) {
            Statements.Add(node);
        }
        public override void Visit(TargetDeclaration node) {
            Statements.Add(node);
        }
        public override void Visit(TargetRecoveryTimeDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(TemporalClause node) {
            Statements.Add(node);
        }
        public override void Visit(ThrowStatement node) {
            Statements.Add(node);
        }
        public override void Visit(TopRowFilter node) {
            Statements.Add(node);
        }
        public override void Visit(TriggerAction node) {
            Statements.Add(node);
        }
        public override void Visit(TriggerObject node) {
            Statements.Add(node);
        }
        public override void Visit(TriggerOption node) {
            Statements.Add(node);
        }
        public override void Visit(TruncateTableStatement node) {
            Statements.Add(node);
        }
        public override void Visit(TruncateTargetTableSwitchOption node) {
            Statements.Add(node);
        }
        public override void Visit(TryCastCall node) {
            Statements.Add(node);
        }
        public override void Visit(TryCatchStatement node) {
            Statements.Add(node);
        }
        public override void Visit(TryConvertCall node) {
            Statements.Add(node);
        }
        public override void Visit(TryParseCall node) {
            Statements.Add(node);
        }
        public override void Visit(TSEqualCall node) {
            Statements.Add(node);
        }
        public override void Visit(TSqlBatch node) {
            Statements.Add(node);
        }
        public override void Visit(TSqlFragment fragment) {
            base.Visit(fragment);
        }
        public override void Visit(TSqlFragmentSnippet node) {
            Statements.Add(node);
        }
        public override void Visit(TSqlScript node) {
            Statements.Add(node);
        }
        public override void Visit(TSqlStatementSnippet node) {
            Statements.Add(node);
        }
        public override void Visit(UnaryExpression node) {
            Statements.Add(node);
        }
        public override void Visit(UniqueConstraintDefinition node) {
            Statements.Add(node);
        }
        public override void Visit(UnpivotedTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(UnqualifiedJoin node) {
            Statements.Add(node);
        }
        public override void Visit(UpdateCall node) {
            Statements.Add(node);
        }
        public override void Visit(UpdateForClause node) {
            Statements.Add(node);
        }
        public override void Visit(UpdateMergeAction node) {
            Statements.Add(node);
        }
        public override void Visit(UpdateSpecification node) {
            Statements.Add(node);
        }
        public override void Visit(UpdateStatisticsStatement node) {
            Statements.Add(node);
        }
        public override void Visit(UpdateTextStatement node) {
            Statements.Add(node);
        }
        public override void Visit(UseFederationStatement node) {
            Statements.Add(node);
        }
        public override void Visit(UseHintList node) {
            Statements.Add(node);
        }
        public override void Visit(UserDataTypeReference node) {
            Statements.Add(node);
        }
        public override void Visit(UserDefinedTypeCallTarget node) {
            Statements.Add(node);
        }
        public override void Visit(UserDefinedTypePropertyAccess node) {
            Statements.Add(node);
        }
        public override void Visit(UserLoginOption node) {
            Statements.Add(node);
        }
        public override void Visit(UserRemoteServiceBindingOption node) {
            Statements.Add(node);
        }
        public override void Visit(UseStatement node) {
            Statements.Add(node);
        }
        public override void Visit(ValuesInsertSource node) {
            Statements.Add(node);
        }
        public override void Visit(VariableMethodCallTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(VariableReference node) {
            Statements.Add(node);
        }
        public override void Visit(VariableTableReference node) {
            Statements.Add(node);
        }
        public override void Visit(VariableValuePair node) {
            Statements.Add(node);
        }
        public override void Visit(ViewDistributionOption node) {
            Statements.Add(node);
        }
        public override void Visit(ViewForAppendOption node) {
            Statements.Add(node);
        }
        public override void Visit(ViewHashDistributionPolicy node) {
            Statements.Add(node);
        }
        public override void Visit(ViewOption node) {
            Statements.Add(node);
        }
        public override void Visit(ViewRoundRobinDistributionPolicy node) {
            Statements.Add(node);
        }
        public override void Visit(WaitAtLowPriorityOption node) {
            Statements.Add(node);
        }
        public override void Visit(WhereClause node) {
            Statements.Add(node);
        }
        public override void Visit(WaitForStatement node) {
            Statements.Add(node);
        }
        public override void Visit(WhileStatement node) {
            Statements.Add(node);
        }
        public override void Visit(WindowDelimiter node) {
            Statements.Add(node);
        }
        public override void Visit(WindowsCreateLoginSource node) {
            Statements.Add(node);
        }
        public override void Visit(WindowFrameClause node) {
            Statements.Add(node);
        }
        public override void Visit(WithCtesAndXmlNamespaces node) {
            Statements.Add(node);
        }
        public override void Visit(WithinGroupClause node) {
            Statements.Add(node);
        }
        public override void Visit(WitnessDatabaseOption node) {
            Statements.Add(node);
        }
        public override void Visit(WlmTimeLiteral node) {
            Statements.Add(node);
        }
        public override void Visit(WorkloadGroupImportanceParameter node) {
            Statements.Add(node);
        }
        public override void Visit(WorkloadGroupResourceParameter node) {
            Statements.Add(node);
        }
        public override void Visit(WriteTextStatement node) {
            Statements.Add(node);
        }
        public override void Visit(WsdlPayloadOption node) {
            Statements.Add(node);
        }
        public override void Visit(XmlDataTypeReference node) {
            Statements.Add(node);
        }
        public override void Visit(XmlForClause node) {
            Statements.Add(node);
        }
        public override void Visit(XmlForClauseOption node) {
            Statements.Add(node);
        }
        public override void Visit(XmlNamespaces node) {
            Statements.Add(node);
        }
        public override void Visit(XmlNamespacesAliasElement node) {
            Statements.Add(node);
        }
        public override void Visit(XmlNamespacesDefaultElement node) {
            Statements.Add(node);
        }

    }
}
