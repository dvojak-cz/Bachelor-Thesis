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


## Guide
Before installing make sure that you have installed [*multus CNI*](https://github.com/k8snetworkplumbingwg/multus-cni) plugin. Also apply taints for edge nodes.
### 1. Install multus CNI plugin and setup taints for edgeNone
```bash
kubectl apply -f manifests/operator/
```

### 2. Install operator
To install the operator download latest release files config. This archive contains manifests for installing the operator.

Manifests files require PKI certificates, keys... You can generate them using cfssl.

Before installing operator make sure that there are no *validatingwebhookconfigurations* and *mutatingWebhookconfiguration* installed on the cluster. Those objects might collide with those that operator uses - that could affect the run of operator.
```bash
# Download latest version of operator
wget https://github.com/dvojak-cz/Bachelor-Thesis/releases/latest/download/config.tar.gz
#wget https://github.com/dvojak-cz/Bachelor-Thesis/releases/download/v0.0.1/config.tar.gz

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

### Test
To verify, that installation has been successful, make sure that everything in `edgeoperator-system` namespace works fine
```bash
edgeoperator-system
```

---
## Links
1. [**BACK** - Lab setup](lab-set-up.md)

1. [**NEXT** - Usage Example](example.md)
1. [**HOME**](README.md)