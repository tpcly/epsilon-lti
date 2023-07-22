FROM node:16 AS Build

WORKDIR /build
EXPOSE 443
EXPOSE 80

RUN curl -f https://get.pnpm.io/v6.16.js | node - add --global pnpm

COPY pnpm-lock.yaml ./

ADD . ./
RUN pnpm install
ENTRYPOINT ["pnpm", "run", "host"]