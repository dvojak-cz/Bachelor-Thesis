


```bash
# TODO PRAVA
```
```bash
cd vagrant
vagrant ssh device01-01
docker compose up
```

```bash
kubectl apply -f doc/sample/nad.yaml
kubectl apply -f doc/sample/device.yaml
kubectl apply -f doc/sample/sample-connection.yaml
```



```bash
kubectl apply -f doc/sample/dbg.yaml
kubectl exec -it debug -- bash
nc dop-sample-connection 8080
nc -u dop-sample-connection 9090
curl dop-sample-connection:8000
```