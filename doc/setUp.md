## Requirements
- virtualbox
- ansible 
- vagrant
- kubectl
- yq

### 0. pre setup
```bash
echo '* 0.0.0.0/0 ::/0' | sudo tee -a /etc/vbox/networks.conf   # allow full range of IPv4 for VMs
ansible-galaxy install -r ansible/requirements.yaml             # install ansible requirements
cat ~/.ssh/id_rsa.pub >> ./vagrant/.ssh_public_keys             # set list of public keys to import to VM
```

### 1. Set up VMs
```bash
cd vagrant
vagrant up --provider=virtualbox && vagrant snapshot save vagrant-up
```

### 2. Prepare VMs
```bash
cd ansible
ansible-playbook playbook/infra.yaml -i inventory/inv && vagrant snapshot save ansible-infra
ansible-playbook playbook/bt.yaml -i inventory/inv && vagrant snapshot save ansible-bt
ansible-playbook playbook/device.yaml -i inventory/inv && vagrant snapshot save ansible-device
```

### 3. Set up k8s
```bash
cd vagrant
sudo su - kube
sudo kubeadm config images pull
sudo kubeadm init --apiserver-advertise-address=172.16.16.100 --apiserver-cert-extra-sans=bt,10.38.6.86 --pod-network-cidr=192.168.0.0/16 | tee -a ~/kubeinit.log
sudo kubeadm token create --print-join-command | tee ~/joincluster.sh
ln -s /etc/kubernetes/admin.conf ~/.kube/config
exit
sudo apt install acl && sudo setfacl -m u:kube:r /etc/kubernetes/admin.conf
exit
../scripts/joinClusterWithNodes.sh
vagrant snapshot save k8s-init
```

### 4. Set up local env
```bash
cd vagrant
../scripts/locaKubectl.sh
kubectl config use-context bt_trojaj12
```

### 5. Set up k8s
```bash
kubectl apply -f manifests/deployAll/
```
