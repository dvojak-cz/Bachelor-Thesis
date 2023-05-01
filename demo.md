#
```bash
ssh bt
cd Bachelor-Thesis/vagrant
vagrant status
vagrant ssh device01
docker compose up
#new window
k get nodes
kubectl get -n edgeoperator-system all
#show manifests
kubectl apply -f doc/sample/nad.yaml
k get nads
kubectl apply -f doc/sample/device.yaml
k get device
kubectl apply -f doc/sample/sample-connection.yaml
k get connection
#


kubectl apply -f doc/sample/dbg.yaml
kubectl exec -it debug -- bash
#nc dop-sample-connection 8080
#nc -u dop-sample-connection 9090
#curl dop-sample-connection:8000
```


---
```bash
# Login pods
kubectl create secret generic regcred \
    --from-file=.dockerconfigjson=/home/jan/.docker/config.json \
    --type=kubernetes.io/dockerconfigjson

# Deployment
yq '.spec.template.spec.imagePullSecrets = [{"name":"regcred"}]' | k apply -f -

# Pod
yq '.spec.imagePullSecrets = [{"name":"regcred"}]' | k apply -f -
```