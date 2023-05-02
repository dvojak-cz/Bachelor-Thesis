## Uvod
- [Zadani](https://github.com/dvojak-cz/Bachelor-Thesis/blob/master/text/trojaj12-assignment.pdf)
- Ukazka prosterdi ([web](https://bt.project.dvojak.cz/example.html))
- [Vagrant](https://bt.project.dvojak.cz/lab-set-up.html)
- [Ansible](https://bt.project.dvojak.cz/lab-set-up.html)

## Operator
- [Device](https://bt.project.dvojak.cz/device.html)
- [Connection](https://bt.project.dvojak.cz/connection.html)
- [Konfigurace](https://bt.project.dvojak.cz/operator-configuration.html)
---
## Instalace
- CNI flannel
    ```bash
    vagrant ssh kedge1
    sudo bash
    ls -la /etc/cni/net.d/
    cat /etc/cni/net.d/10*
    ```
1. [Operator Install](https://bt.project.dvojak.cz/operator-install.html)
    - [Release](https://github.com/dvojak-cz/Bachelor-Thesis/releases)

    - `manifests/operator/` (labels, multus)
        ```bash
        kubectl apply -f manifests/operator/
        ```
    - install
        ```bash
        cat /etc/cni/net.d/00*
        ```
---
## Ukazka

- **DOCKER**

```bash
k explain device
k explain connection

kubectl apply -f doc/sample/nad.yaml
k get network-attachment-definitions

kubectl apply -f doc/sample/device.yaml
k get device  -o wide

kubectl apply -f doc/sample/sample-connection.yaml
k get connection  -o wide
k get pods -o wide
k get service -o wide
```



```bash
kubectl apply -f doc/sample/dbg.yaml
kubectl exec -it debug -- bash
nc dop-sample-connection 8080
nc -u dop-sample-connection 9090
curl dop-sample-connection:8000
```
---
## Strict mode
```bash
kubectl apply -f doc/sample/extra/config.yaml

kubectl delete validatingwebhookconfigurations --all
kubectl delete mutatingwebhookconfigurations --all

k get -n edgeoperator-system deployments.apps edgeoperator-operator -o yaml | \
yq e '.spec.template.spec.containers[0].envFrom += [{"configMapRef": {"name":"strict-device"}}]' | \
k apply -f -

k apply 

```



---
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