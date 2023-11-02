import type { JwtPayload } from "jwt-decode"

export interface LtiMessage extends JwtPayload {
	type: MessageType
	version: string
	deploymentId: string
	resourceLink: ResourceLink
	targetLinkUrl: URL
	lis: LearningInformationServices
	context: Context
	toolPlatform: ToolPlatformReference
	launchPresentation: LaunchPresentation
	role: string[]
	deepLinkingSettings: DeepLinkingSettings | undefined
	custom: { [key: string]: string }
	nonce: string
}

type MessageType =
	| "LtiResourceLinkRequest"
	| "LtiDeepLinkingRequest"
	| "LtiSubmissionReviewRequest"

type ResourceLink = {
	id: string
	title: string
	description: string | undefined
}

type LearningInformationServices = {
	personSourceId: string | undefined
	courseOfferingSourcedId: string | undefined
	courseSectionSourcedId: string | undefined
}

type Context = {
	id: string
	label: string
	title: string
	type: ContextType[]
}

type ContextType =
	| "http://purl.imsglobal.org/vocab/lis/v2/course#CourseTemplate"
	| "http://purl.imsglobal.org/vocab/lis/v2/course#CourseOffering"
	| "http://purl.imsglobal.org/vocab/lis/v2/course#CourseSection"
	| "http://purl.imsglobal.org/vocab/lis/v2/course#Group"

type ToolPlatformReference = {
	guid: string
	name: string
	version: string
	productFamilyCode: string
}

type LaunchPresentation = {
	documentTarget: DocumentTarget
	width: number
	height: number
	locale: string
	returnUrl: string
}

type DocumentTarget = "iframe" | "window"

type DeepLinkingSettings = {
	deepLinkReturnUrl: string
	acceptTypes: string[]
	acceptPresentationDocumentTargets: string[]
	acceptMediaTypes: string
	autoCreate: boolean
	acceptMultiple: boolean
}
