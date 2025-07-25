FROM node:18-alpine as build

WORKDIR /app
COPY src/Frontend/package.json ./
#RUN npm ci --only=production
RUN npm install

COPY src/Frontend ./
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
COPY src/Frontend/nginx.conf /etc/nginx/nginx.conf

EXPOSE 3000
CMD ["nginx", "-g", "daemon off;"]
