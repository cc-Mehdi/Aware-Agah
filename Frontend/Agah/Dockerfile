FROM node:20-alpine

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

# Expose Vite's default port (5173)
EXPOSE 5173

# Start Vite dev server
CMD ["npm", "run", "dev"]