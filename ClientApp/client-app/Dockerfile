FROM node:latest AS build
WORKDIR /usr/src/app
COPY ./ .
RUN npm install
RUN npm run build --prod

FROM nginx:latest
COPY --from=build /usr/src/app/dist/client-app /usr/share/nginx/html
EXPOSE 4200
