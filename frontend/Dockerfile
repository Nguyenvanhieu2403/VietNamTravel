# Stage 1: Build
FROM node:20-alpine AS build
WORKDIR /app

# Copy dependency files
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy source code files
COPY . .

# Compile application (builds client and server bundles)
RUN npm run build

# Stage 2: Runtime
FROM node:20-alpine AS runtime
WORKDIR /app

# Copy compiled bundles from build stage
COPY --from=build /app/dist/frontend ./dist/frontend

# Expose SSR Node server port
EXPOSE 4000

# Set environment variables
ENV PORT=4000
ENV NODE_ENV=production

# Run the Angular SSR server
CMD ["node", "dist/frontend/server/server.mjs"]
