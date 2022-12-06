FROM node:16.18.1-alpine3.15 AS image-build
RUN mkdir -p /app
WORKDIR /app
COPY package.json /app
RUN npm install
COPY . /app
RUN npm run build

# Stage 2
FROM nginx:1.19.1-alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=image-build /app/dist/k8sTest /usr/share/nginx/html
