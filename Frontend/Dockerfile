# Build image
FROM node:9-alpine as builder

COPY package.json package-lock.json ./
RUN npm set progress=false && npm config set depth 0 && npm cache clean --force
RUN npm i && mkdir /ng-app && cp -R ./node_modules ./ng-app

WORKDIR /ng-app
COPY . .

# a simple workaround to sove env url issue, altough it violates the build once deploy many principle
ARG IS_CLOUD 
RUN if [ "x$IS_CLOUD" = "x" ] ; then $(npm bin)/ng build ; else $(npm bin)/ng build --configuration=cloud; fi

# Runtime image
FROM nginx:1.13.3-alpine

COPY nginx/default.conf /etc/nginx/conf.d/

## Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*

COPY --from=builder /ng-app/dist /usr/share/nginx/html

CMD ["nginx", "-g", "daemon off;"]