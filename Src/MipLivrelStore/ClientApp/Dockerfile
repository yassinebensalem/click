# Build stage for Development
FROM node:14 as build

WORKDIR /app

# Set environment variable to development
ENV NODE_ENV=development

# Copy package.json and package-lock.json to the container
COPY package*.json ./

# Install all dependencies, including devDependencies
RUN npm install

# Copy the rest of the application files to the container
COPY . .

# Build the React app in development mode (depending on your setup, you might not need a "build" step for development)
RUN npm run build

# "Runtime" stage for Development using Nginx
FROM nginx:alpine

# Copy the built artifacts from the build-dev stage to the Nginx server
COPY --from=build /app/build /usr/share/nginx/html

# Expose the default Nginx port
EXPOSE 80

# Start Nginx when the container runs
CMD ["nginx", "-g", "daemon off;"]
