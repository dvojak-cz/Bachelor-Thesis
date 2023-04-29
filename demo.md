


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