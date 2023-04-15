# Operator deployment
At the end of this guide you should have running edge-operator on your cluster. Once its done you can start using it.

## Requirements
- [cfssl](https://github.com/cloudflare/cfssl)
- [cfssljson](https://github.com/cloudflare/cfssl)
- [yq](https://github.com/mikefarah/yq)
- kubectl

You can install `yq`, `cfssl`, `cfssljson` using folowing commands.
```bash
go install github.com/cloudflare/cfssl/cmd/cfssl@1.6.3
go install github.com/cloudflare/cfssl/cmd/cfssljson@1.6.3
go install github.com/mikefarah/yq/v4@v4.33.2
```
### Configuration
Operator is highly configurable, for more information see [OperatorConfiguration](operator-configuration.md) page.

---
### 1. Install multus CNI plugin and setup taints for edgeNone
```bash
kubectl apply -f manifests/operator/
```

### 2. Install operator
```bash
# Download latest version of operator
wget https://github.com/dvojak-cz/Bachelor-Thesis/releases/latest/download/config.tar.gz
tar -xf config.tar.gz
cd config/operator

cat > csr.json <<EOF
{
  "CN": "Operator Root CA",
  "key": {
    "algo": "rsa",
    "size": 2048
  },
  "names": [
    {
      "C": "DEV",
      "L": "Kubernetes",
      "O": "Kubernetes Operator"
    }
  ]
}
EOF

cfssl gencert -initca csr.json | cfssljson -bare ca -   # Generate self-signed certificate
cd ..

kubectl delete validatingwebhookconfigurations --all
kubectl delete mutatingWebhookconfiguration --all



kubectl apply -k install/                               # Deploy operator
```