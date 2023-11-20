FROM node:18 AS Build

WORKDIR /build

EXPOSE 443
EXPOSE 80

ENV NUXT_HOST=0.0.0.0
ENV PORT=80

RUN curl -f https://get.pnpm.io/v6.16.js | node - add --global pnpm
COPY pnpm-lock.yaml ./

ADD . ./
RUN pnpm install

RUN pnpm run build

ENTRYPOINT ["pnpm", "run", "start"]