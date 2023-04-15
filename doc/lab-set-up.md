# Lab setup
At the end of this guide you should have up end running kubernetes cluster for demo purposes

This guide helps you with setting up lab for demo.
Guide was tested with `virtualbox-6.1`, `vagrant-2.3.4`, `debian-11`

## Requirements
- [virtualbox](https://www.virtualbox.org/) (or other compatible VMM)
- [ansible](https://www.ansible.com/)
- [vagrant](https://www.vagrantup.com/)
- kubectl

---

### 0. Setting up host
```bash
echo '* 0.0.0.0/0 ::/0' | sudo tee -a /etc/vbox/networks.conf   # allow full range of IPv4 for VMs
ansible-galaxy install -r ansible/requirements.yaml             # install ansible requirements
cat ~/.ssh/id_rsa.pub >> ./vagrant/.ssh_public_keys             # set list of public keys to import to VM
```

### 1. Create VMs
```bash
cd vagrant
vagrant up  # create and run VMs
```

### 2. Setup VMs
```bash
cd ansible
ansible-playbook playbook/infra.yaml -i inventory/inv   # setup cluster nodes
ansible-playbook playbook/device.yaml -i inventory/inv  # setup demo on devices
```

### 3. Set up k8s
```bash
cd vagrant
vagrant ssh kmaster
sudo su - kube
sudo kubeadm config images pull
# create kubernetes cluster
# set kubernetes API endpoint
# add records about other IPs to certificate (add IPs and DNS names you will be using for connecting to cluster)
# set IP range for PODs, depends on used CNI and its configuration
sudo kubeadm init \
    --apiserver-advertise-address=172.16.16.100 \
    --apiserver-cert-extra-sans=bt,10.38.6.86 \
    --pod-network-cidr=10.244.0.0/16 \
    | tee -a ~/kubeinit.log
sudo kubeadm token create --print-join-command \
    | tee ~/joincluster.sh                          # store connection string !DON'T USE FOR PRODUCTION!
ln -s /etc/kubernetes/admin.conf ~/.kube/config
exit
sudo chmod +r /etc/kubernetes/admin.conf
exit
../scripts/joinClusterWithNodes.sh                  # add other nodes to cluster
```

### 4. Set up local env
```bash
cd vagrant
../scripts/locaKubectl.sh                           # update your kubectl config for newly created kubernetes cluster
kubectl config use-context bt_trojaj12              # switch context to newly created kubernetes cluster
```

### 5. Set up k8s
```bash
kubectl apply -f manifests/lab/               # setup cluster
```

---

Now you should have running kubernetes cluster, you can test that by running
```bash
kubectl get nodes
```