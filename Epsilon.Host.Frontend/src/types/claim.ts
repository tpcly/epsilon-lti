type Context = "https://purl.imsglobal.org/spec/lti/claim/context"
type Custom = "https://purl.imsglobal.org/spec/lti/claim/custom"
type DeepLinkingSettings =
	"https://purl.imsglobal.org/spec/lti/claim/deep_link_settings"
type DeploymentId = "https://purl.imsglobal.org/spec/lti/claim/deployment_id"
type LaunchPresentation =
	"https://purl.imsglobal.org/spec/lti/claim/launch_presentation"
type Lis = "https://purl.imsglobal.org/spec/lti/claim/lis"
type ResourceLink = "https://purl.imsglobal.org/spec/lti/claim/resource_link"
type Roles = "https://purl.imsglobal.org/spec/lti/claim/roles"
type TargetLinkUri = "https://purl.imsglobal.org/spec/lti/claim/target_link_uri"
type ToolPlatform = "https://purl.imsglobal.org/spec/lti/claim/tool_platform"
type MessageType = "https://purl.imsglobal.org/spec/lti/claim/message_type"
type Version = "https://purl.imsglobal.org/spec/lti/claim/version"

export type Claim =
	| Context
	| Custom
	| DeepLinkingSettings
	| DeploymentId
	| LaunchPresentation
	| Lis
	| ResourceLink
	| Roles
	| TargetLinkUri
	| ToolPlatform
	| MessageType
	| Version
