/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface EnrollmentTerm {
	name?: string | null
	/** @format date-time */
	start_at?: string | null
	/** @format date-time */
	end_at?: string | null
}

export interface LearningDomain {
	id?: string | null
	rowsSet: LearningDomainTypeSet
	columnsSet?: LearningDomainTypeSet
	valuesSet: LearningDomainTypeSet
}

export interface LearningDomainCriteria {
	/** @format int32 */
	id?: number
	/** @format double */
	masteryPoints?: number | null
}

export interface LearningDomainOutcome {
	/** @format int32 */
	id?: number
	row: LearningDomainType
	column?: LearningDomainType
	value: LearningDomainType
	name: string
}

export interface LearningDomainOutcomeRecord {
	outcome?: LearningDomainOutcome
	/** @format double */
	grade?: number | null
}

export interface LearningDomainSubmission {
	assignment?: string | null
	/** @format uri */
	assignmentUrl?: string | null
	/** @format date-time */
	submittedAt?: string
	criteria?: LearningDomainCriteria[] | null
	results?: LearningDomainOutcomeRecord[] | null
}

export interface LearningDomainType {
	id?: string | null
	name: string
	shortName: string
	hexColor?: string | null
	/** @format int32 */
	order?: number
}

export interface LearningDomainTypeSet {
	/** @format uuid */
	id?: string
	types: LearningDomainType[]
}

export interface PageComponent {
	html?: string | null
}

export interface PageUpdateRequest {
	body: string
}

export interface User {
	_id?: string | null
	name?: string | null
	/** @format uri */
	avatarUrl?: string | null
}

export type QueryParamsType = Record<string | number, any>
export type ResponseFormat = keyof Omit<Body, "body" | "bodyUsed">

export interface FullRequestParams extends Omit<RequestInit, "body"> {
	/** set parameter to `true` for call `securityWorker` for this request */
	secure?: boolean
	/** request path */
	path: string
	/** content type of request body */
	type?: ContentType
	/** query params */
	query?: QueryParamsType
	/** format of response (i.e. response.json() -> format: "json") */
	format?: ResponseFormat
	/** request body */
	body?: unknown
	/** base url */
	baseUrl?: string
	/** request cancellation token */
	cancelToken?: CancelToken
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">

export interface ApiConfig<SecurityDataType = unknown> {
	baseUrl?: string
	baseApiParams?: Omit<RequestParams, "baseUrl" | "cancelToken" | "signal">
	securityWorker?: (securityData: SecurityDataType | null) => Promise<RequestParams | void> | RequestParams | void
	customFetch?: typeof fetch
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown> extends Response {
	data: D
	error: E
}

type CancelToken = Symbol | string | number

export enum ContentType {
	Json = "application/json",
	FormData = "multipart/form-data",
	UrlEncoded = "application/x-www-form-urlencoded",
	Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
	public baseUrl: string = ""
	private securityData: SecurityDataType | null = null
	private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"]
	private abortControllers = new Map<CancelToken, AbortController>()
	private customFetch = (...fetchParams: Parameters<typeof fetch>) => fetch(...fetchParams)

	private baseApiParams: RequestParams = {
		credentials: "same-origin",
		headers: {},
		redirect: "follow",
		referrerPolicy: "no-referrer",
	}

	constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
		Object.assign(this, apiConfig)
	}

	public setSecurityData = (data: SecurityDataType | null) => {
		this.securityData = data
	}

	protected encodeQueryParam(key: string, value: any) {
		const encodedKey = encodeURIComponent(key)
		return `${encodedKey}=${encodeURIComponent(typeof value === "number" ? value : `${value}`)}`
	}

	protected addQueryParam(query: QueryParamsType, key: string) {
		return this.encodeQueryParam(key, query[key])
	}

	protected addArrayQueryParam(query: QueryParamsType, key: string) {
		const value = query[key]
		return value.map((v: any) => this.encodeQueryParam(key, v)).join("&")
	}

	protected toQueryString(rawQuery?: QueryParamsType): string {
		const query = rawQuery || {}
		const keys = Object.keys(query).filter((key) => "undefined" !== typeof query[key])
		return keys
			.map((key) => (Array.isArray(query[key]) ? this.addArrayQueryParam(query, key) : this.addQueryParam(query, key)))
			.join("&")
	}

	protected addQueryParams(rawQuery?: QueryParamsType): string {
		const queryString = this.toQueryString(rawQuery)
		return queryString ? `?${queryString}` : ""
	}

	private contentFormatters: Record<ContentType, (input: any) => any> = {
		[ContentType.Json]: (input: any) =>
			input !== null && (typeof input === "object" || typeof input === "string") ? JSON.stringify(input) : input,
		[ContentType.Text]: (input: any) => (input !== null && typeof input !== "string" ? JSON.stringify(input) : input),
		[ContentType.FormData]: (input: any) =>
			Object.keys(input || {}).reduce((formData, key) => {
				const property = input[key]
				formData.append(
					key,
					property instanceof Blob
						? property
						: typeof property === "object" && property !== null
						? JSON.stringify(property)
						: `${property}`
				)
				return formData
			}, new FormData()),
		[ContentType.UrlEncoded]: (input: any) => this.toQueryString(input),
	}

	protected mergeRequestParams(params1: RequestParams, params2?: RequestParams): RequestParams {
		return {
			...this.baseApiParams,
			...params1,
			...(params2 || {}),
			headers: {
				...(this.baseApiParams.headers || {}),
				...(params1.headers || {}),
				...((params2 && params2.headers) || {}),
			},
		}
	}

	protected createAbortSignal = (cancelToken: CancelToken): AbortSignal | undefined => {
		if (this.abortControllers.has(cancelToken)) {
			const abortController = this.abortControllers.get(cancelToken)
			if (abortController) {
				return abortController.signal
			}
			return void 0
		}

		const abortController = new AbortController()
		this.abortControllers.set(cancelToken, abortController)
		return abortController.signal
	}

	public abortRequest = (cancelToken: CancelToken) => {
		const abortController = this.abortControllers.get(cancelToken)

		if (abortController) {
			abortController.abort()
			this.abortControllers.delete(cancelToken)
		}
	}

	public request = async <T = any, E = any>({
		body,
		secure,
		path,
		type,
		query,
		format,
		baseUrl,
		cancelToken,
		...params
	}: FullRequestParams): Promise<HttpResponse<T, E>> => {
		const secureParams =
			((typeof secure === "boolean" ? secure : this.baseApiParams.secure) &&
				this.securityWorker &&
				(await this.securityWorker(this.securityData))) ||
			{}
		const requestParams = this.mergeRequestParams(params, secureParams)
		const queryString = query && this.toQueryString(query)
		const payloadFormatter = this.contentFormatters[type || ContentType.Json]
		const responseFormat = format || requestParams.format

		return this.customFetch(`${baseUrl || this.baseUrl || ""}${path}${queryString ? `?${queryString}` : ""}`, {
			...requestParams,
			headers: {
				...(requestParams.headers || {}),
				...(type && type !== ContentType.FormData ? { "Content-Type": type } : {}),
			},
			signal: (cancelToken ? this.createAbortSignal(cancelToken) : requestParams.signal) || null,
			body: typeof body === "undefined" || body === null ? null : payloadFormatter(body),
		}).then(async (response) => {
			const r = response as HttpResponse<T, E>
			r.data = null as unknown as T
			r.error = null as unknown as E

			const data = !responseFormat
				? r
				: await response[responseFormat]()
						.then((data) => {
							if (r.ok) {
								r.data = data
							} else {
								r.error = data
							}
							return r
						})
						.catch((e) => {
							r.error = e
							return r
						})

			if (cancelToken) {
				this.abortControllers.delete(cancelToken)
			}

			if (!response.ok) throw data
			return data
		})
	}
}

/**
 * @title Epsilon.Host.WebApi
 * @version 1.0
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
	document = {
		/**
		 * No description
		 *
		 * @tags Document
		 * @name PageDetail
		 * @request GET:/Document/page/{pageName}
		 */
		pageDetail: (
			pageName: string,
			query?: {
				/** @format int32 */
				courseId?: number
			},
			params: RequestParams = {}
		) =>
			this.request<PageComponent, any>({
				path: `/Document/page/${pageName}`,
				method: "GET",
				query: query,
				format: "json",
				...params,
			}),

		/**
		 * No description
		 *
		 * @tags Document
		 * @name PageCreate
		 * @request POST:/Document/page/{pageName}
		 */
		pageCreate: (
			pageName: string,
			data: PageUpdateRequest,
			query?: {
				/** @format int32 */
				courseId?: number
			},
			params: RequestParams = {}
		) =>
			this.request<PageComponent, any>({
				path: `/Document/page/${pageName}`,
				method: "POST",
				query: query,
				body: data,
				type: ContentType.Json,
				format: "json",
				...params,
			}),

		/**
		 * No description
		 *
		 * @tags Document
		 * @name DownloadWordList
		 * @request GET:/Document/download/word
		 */
		downloadWordList: (
			query?: {
				/** @format int32 */
				courseId?: number
				/** @format date-time */
				from?: string
				/** @format date-time */
				to?: string
			},
			params: RequestParams = {}
		) =>
			this.request<void, any>({
				path: `/Document/download/word`,
				method: "GET",
				query: query,
				...params,
			}),
	}
	filter = {
		/**
		 * No description
		 *
		 * @tags Filter
		 * @name ParticipatedTermsList
		 * @request GET:/Filter/participated-terms
		 */
		participatedTermsList: (
			query?: {
				studentId?: string
			},
			params: RequestParams = {}
		) =>
			this.request<EnrollmentTerm[], any>({
				path: `/Filter/participated-terms`,
				method: "GET",
				query: query,
				format: "json",
				...params,
			}),

		/**
		 * No description
		 *
		 * @tags Filter
		 * @name AccessibleStudentsList
		 * @request GET:/Filter/accessible-students
		 */
		accessibleStudentsList: (params: RequestParams = {}) =>
			this.request<User[], any>({
				path: `/Filter/accessible-students`,
				method: "GET",
				format: "json",
				...params,
			}),
	}
	learning = {
		/**
		 * No description
		 *
		 * @tags Learning
		 * @name OutcomesList
		 * @request GET:/Learning/outcomes
		 */
		outcomesList: (
			query?: {
				studentId?: string
			},
			params: RequestParams = {}
		) =>
			this.request<LearningDomainSubmission[], any>({
				path: `/Learning/outcomes`,
				method: "GET",
				query: query,
				format: "json",
				...params,
			}),

		/**
		 * No description
		 *
		 * @tags Learning
		 * @name DomainDetail
		 * @request GET:/Learning/domain/{name}
		 */
		domainDetail: (name: string, params: RequestParams = {}) =>
			this.request<LearningDomain, any>({
				path: `/Learning/domain/${name}`,
				method: "GET",
				format: "json",
				...params,
			}),

		/**
		 * No description
		 *
		 * @tags Learning
		 * @name DomainOutcomesList
		 * @request GET:/Learning/domain/outcomes
		 */
		domainOutcomesList: (params: RequestParams = {}) =>
			this.request<LearningDomainOutcome, any>({
				path: `/Learning/domain/outcomes`,
				method: "GET",
				format: "json",
				...params,
			}),
	}
}
