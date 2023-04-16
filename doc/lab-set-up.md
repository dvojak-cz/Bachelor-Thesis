# Lab setup
At the end of this guide you should have up end running kubernetes cluster for demo purposes

This guide helps you with setting up lab for demo.
Guide was tested using: `virtualbox-6.1`, `vagrant-2.3.4`, `debian-11`

## Requirements
- [virtualbox](https://www.virtualbox.org/) (or other compatible VMM)
- [ansible](https://www.ansible.com/)
- [vagrant](https://www.vagrantup.com/)
- kubectl

## Guide

### 0. Setting up host
In case you are using virtualbox, make sure you have enabled the creation of the necessary ip ranges

```bash
echo '* 0.0.0.0/0 ::/0' | sudo tee -a /etc/vbox/networks.conf   # allow full range of IPv4 for VMs
```
Install required ansible packages
```bash
ansible-galaxy install -r ansible/requirements.yaml             # install ansible requirements
```
Add all public SSH keys, that you want to import to VMs,  to `vagrant/.ssh_public_keys`. If you don't want to add any, leave the file empty.
```bash
cat ~/.ssh/id_rsa.pub >> ./vagrant/.ssh_public_keys             # set list of public keys to import
```

### 1. Create VMs
Use vagrant to create all VMs. If you want to add or remove any machine, fell fre to customize `vagrant/Vagrantfile`
```bash
cd vagrant
vagrant up  # create and run VMs
```

### 2. Setup VMs
Install and configure VMs. You can use provided playbooks from `ansible/playbook`.
```bash
cd ansible
ansible-playbook playbook/infra.yaml -i inventory/inv   # setup cluster nodes
ansible-playbook playbook/device.yaml -i inventory/inv  # setup demo on devices
```

### 3. Set up k8s
To set up k8s cluster, log into `kmaster` machine. Use *kube* user for interacting with k8s. When using `kubeadm` for setting up cluster, make sure you use correct parameter (in case you are not using default settings).

Feel free to add custom IP and DNS names to `apiserver-cert-extra-sans` parameter so you can access k8s cluster from remote locations. Example uses bt (bachelor thesis) DNS name.

Make sure that `/etc/kubernetes/admin.conf` file located on `kmaster` is readable by user *kube*.
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
Flowing script will add credentials for kubectl. Feel free to edit cluster name. You can pass new name as an first (`$1`) argument to script. Example uses *bt_trojaj12* as a name for the cluster.
```bash
cd vagrant
../scripts/locaKubectl.sh bt_trojaj12               # update your kubectl config for newly created kubernetes cluster
kubectl config use-context bt_trojaj12              # switch context to newly created kubernetes cluster
```

### 5. Set up k8s
```bash
kubectl apply -f manifests/lab/               # setup cluster
```

## Test
Now you should have running kubernetes cluster, you can test that by running following command.
```bash
kubectl get nodes
```

---
## Links
2. [**NEXT** - Operator Install](operator-install.md)

1. [**HOME**](README.md)