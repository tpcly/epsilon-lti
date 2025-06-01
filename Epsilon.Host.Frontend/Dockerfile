FROM node:24 AS Build

WORKDIR /build

EXPOSE 443
EXPOSE 80

ENV NUXT_HOST=0.0.0.0
ENV PORT=80

RUN npm install -g pnpm
COPY pnpm-lock.yaml ./

ADD . ./
RUN pnpm install

RUN pnpm run build

ENTRYPOINT ["pnpm", "run", "start"]
