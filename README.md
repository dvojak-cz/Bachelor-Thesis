# Bachelor-Thesis
This repository contains everithing  related to thesis ***Síťová komunikace aplikací v Kubernetes s externími zařízeními v privátní síti*** (*Network communication of Kubernetes applications with external devices in a private network*).

## [Assignment-CZ](./text/trojaj12-assignment.pdf)
Kubernetes se stává přední technologií pro správu aplikací ve formě kontejnerů. Kubernetes řeší komunikaci ve vnitřní síti klastru. V případě potřeby komunikace s externím zařízením, které není a nemůže být součástí sítě klastru, nenabízí technologie Kubernetes jednoduché řešení. Cílem práce je rozšířit funkcionalitu Kubernetes tak, aby umožila komunikaci právě se zařízeními mimo vnitřní síť klastru.
- Prozkoumejte, jaké možnosti pro síťování Kubernetes nabízí.
- Prozkoumejte možnosti adresace a komunikace se zařízeními, které se nachází v privátní síti, mimo síť klastru.
- Zaměřte se na komunikaci pomocí TCP, UDP a HTTP protokolů.
- Navrhněte a implementujte řešení, které umožní navázat komunikaci mezi kontejnery v Kubernetes a zařízeními mimo interní síť klastru.

Implementaci je možné provést čistě ve virtuálním prostředí. V případě implementace ve virtuálním prostředí dodejte kompletní virtuální prostředí nebo definici prostředí.

## Assignment-EN
Kubernetes is becoming the leading technology for managing applications in the form of containers. Kubernetes handles communication within the internal network of the cluster. When it comes to communicating with an external device that is not and cannot be part of the cluster network, Kubernetes technology does not offer a simple solution. The aim of this work is to extend the functionality of Kubernetes to allow communication with devices outside the internal network of the cluster.
- Explore what networking options Kubernetes offers.
- Explore options for addressing and communicating with devices that are on a private network, outside the cluster network.
- Focus on communication using TCP, UDP, and HTTP protocols.
- Design and implement a solution to establish communication between containers in Kubernetes and devices outside the cluster's internal network.

The implementation can be done purely in a virtual environment. If implemented in a virtual environment, provide a complete virtual environment or environment definition.

>Translated with www.DeepL.com/Translator (free version)

## Doc
For documentation please see `doc` dir or [bt.project.dvojak.cz](https://bt.project.dvojak.cz/).

---

## Working with repository
### Cloning
For cloning use following command:
```bash
git clone https://github.com/dvojak-cz/Edge-Operator.git
```

### Submodules
This repository uses submodules for linking repository `dvojak-cz/Edge-Operator`. In case you want to clone source for edge operator use following commands:
```bash
git submodule init code/EdgeOperator
git submodule update code/EdgeOperator
```

### LFS
Since the repository contains binary files like images pdfs -- repository uses Git LFS (Large file versioning).

### Structure
```
├─ ansible/                         definice prostředı́ pomocí ansible
|  ├─ playbook/                     definice prostředı́ pomocí ansible
|  ├─ inventory/                    seznam hosts pro andisble
|  └─ vars/                         proměnné pro ansible playbooks
├─ code/                            zdrojové kódy
|  ├─ EdgeOperator/                 zdrojové kódy operatoru
|  |  ├─ EdgeOperator/              operátor
|  |  ├─ EdgeOperator.Tests/        unit testy
|  |  └─ EdgeOperator.sln
|  └─sampleSrvers/                  zdrojové kódy pomocných programů pro testovánı́
|    ├─ tcpServer/
|    └─ udpServer/
├─ doc/                             návod na instalaci prostředı́ a instalaci operatoru
├─ manifests/                       manifesty pro Kubenretes
|  ├─ lab/
|  └─ operator/
├─ scripts/                         pomocná scripty pro instalaci prostředí
├─ text/                            text bakalářské práce
├─ vagrant/                         adresář obsahujı́cı́ definice virtuálnı́ho prostředı́
├─ .github/                         definice pipelines na GitHub
└─ readme.md                       stručný popis repositáře
```