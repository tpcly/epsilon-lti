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
    rowsSet?: LearningDomainTypeSet
    columnsSet?: LearningDomainTypeSet
    valuesSet?: LearningDomainTypeSet
}

export interface LearningDomainType {
    id?: string | null
    name?: string | null
    shortName?: string | null
    hexColor?: string | null
}

export interface LearningDomainTypeSet {
    /** @format uuid */
    id?: string
    types?: LearningDomainType[] | null
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
            .map((key) =>
                Array.isArray(query[key]) ? this.addArrayQueryParam(query, key) : this.addQueryParam(query, key)
            )
            .join("&")
    }

    protected addQueryParams(rawQuery?: QueryParamsType): string {
        const queryString = this.toQueryString(rawQuery)
        return queryString ? `?${queryString}` : ""
    }

    private contentFormatters: Record<ContentType, (input: any) => any> = {
        [ContentType.Json]: (input: any) =>
            input !== null && (typeof input === "object" || typeof input === "string") ? JSON.stringify(input) : input,
        [ContentType.Text]: (input: any) =>
            input !== null && typeof input !== "string" ? JSON.stringify(input) : input,
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
            signal: cancelToken ? this.createAbortSignal(cancelToken) : requestParams.signal,
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
    component = {
        /**
         * No description
         *
         * @tags Component
         * @name PageDetail
         * @request GET:/Component/page/{pageName}
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
                path: `/Component/page/${pageName}`,
                method: "GET",
                query: query,
                format: "json",
                ...params,
            }),

        /**
         * No description
         *
         * @tags Component
         * @name PageCreate
         * @request POST:/Component/page/{pageName}
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
                path: `/Component/page/${pageName}`,
                method: "POST",
                query: query,
                body: data,
                type: ContentType.Json,
                format: "json",
                ...params,
            }),
    }
    document = {
        /**
         * No description
         *
         * @tags Document
         * @name WordList
         * @request GET:/Document/word
         */
        wordList: (
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
                path: `/Document/word`,
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
    learningDomain = {
        /**
         * No description
         *
         * @tags LearningDomain
         * @name LearningDomainDetail
         * @request GET:/learning-domain/{name}
         */
        learningDomainDetail: (name: string, params: RequestParams = {}) =>
            this.request<LearningDomain, any>({
                path: `/learning-domain/${name}`,
                method: "GET",
                format: "json",
                ...params,
            }),
    }
}
