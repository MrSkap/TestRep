FROM node:latest AS build
WORKDIR /usr/src/app
COPY ./ .
RUN npm install
RUN npm run build


FROM nginx
RUN rm /etc/nginx/conf.d/default.conf
COPY --from=build /usr/src/app/dist/client-app /var/www/html
COPY nginx.conf  /etc/nginx
EXPOSE 80
