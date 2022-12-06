#! /usr/bin/bash

# install ingress nginx controller
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.5.1/deploy/static/provider/cloud/deploy.yaml

# build image for api
docker build -t unikey/test-app-api-k8s Code/ApiDotnet/. -f Code/ApiDotnet/Dockerfile

# build image for client
docker build -t unikey/test-app-client-k8s Code/ClientAngular/. -f Code/ClientAngular/Dockerfile

# create configMaps and secrets
kubectl apply -f ./k8s-configmaps-config.yaml
kubectl apply -f ./k8s-secrets-config.yaml

# create database pod and service
kubectl apply -f ./k8s-postgresql-config.yaml

# create backend pod and service
kubectl apply -f ./k8s-api-config.yaml

# create frontend pod and service
kubectl apply -f ./k8s-client-config.yaml

# create ingress for client and server
kubectl apply -f ./k8s-ingress-config.yaml

# get result
kubectl get all
