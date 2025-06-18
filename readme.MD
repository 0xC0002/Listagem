# MultiFolderSelector

**MultiFolderSelector** é uma ferramenta para Windows que permite copiar a estrutura de pastas e listar arquivos em um formato de texto. 
## Funcionalidades

- **Copiar Estrutura de Pastas:** Seleciona uma ou mais pastas e copia a estrutura de diretórios e os nomes dos arquivos para a área de transferência.
- **Interface de Menu de Contexto:** Acesso direto pelo menu de contexto no Windows Explorer, facilitando o uso sem a necessidade de abrir o programa diretamente.
- **Suporte a Múltiplas Seleções:** Permite selecionar várias pastas ao mesmo tempo.
- **Formato de Saída Personalizado:** Lista os arquivos e subpastas com indentação, facilitando a visualização.

Aqui está um exemplo de saída para o seu software usando cantores da MPB:

## Exemplo de Saída

Ao selecionar uma pasta com o software, você obterá um texto com o formato como este:

```
Caetano Veloso - Abraçaço 2012
  01 - Caetano Veloso - A Bossa Nova é Foda.mp3
  02 - Caetano Veloso - Um Abraçaço.mp3
  03 - Caetano Veloso - Quero Ser Justo.mp3

Gilberto Gil - Refazenda 1975
  01 - Gilberto Gil - Refazenda.mp3
  02 - Gilberto Gil - Ela.mp3
  03 - Gilberto Gil - Retiros Espirituais.mp3

Elis Regina - Falso Brilhante 1976
  01 - Elis Regina - Como Nossos Pais.mp3
  02 - Elis Regina - Velha Roupa Colorida.mp3
  03 - Elis Regina - Los Hermanos.mp3

Chico Buarque - Construção 1971
  01 - Chico Buarque - Construção.mp3
  02 - Chico Buarque - Deus Lhe Pague.mp3
  03 - Chico Buarque - Cotidiano.mp3

Gal Costa - Índia 1973
  01 - Gal Costa - Índia.mp3
  02 - Gal Costa - Milho Verde.mp3
  03 - Gal Costa - Presente Cotidiano.mp3
```


## Requisitos

- **Sistema Operacional:** Windows
- **.NET Framework:** Certifique-se de que o .NET Framework esteja instalado para executar o aplicativo.
- **Permissões de Sistema:** Algumas pastas protegidas podem necessitar de permissões de administrador para serem listadas.

## Instalação

1. Clone o repositório para o seu computador:
   ```bash
   git clone https://github.com/0xC0002/Listagem.git
   ```

2. Abra o projeto no Visual Studio e compilar o projeto.

3. Para adicionar o software ao menu de contexto do Windows, siga os passos na seção **Configuração do Menu de Contexto**.

## Como Usar

1. **Selecione a Pasta**:
   Clique com o botão direito em uma pasta no Windows Explorer e selecione a opção do menu de contexto para listar a estrutura da pasta.

2. **Copiar a Estrutura**:
   O software irá copiar a estrutura para a área de transferência, que poderá ser colada em qualquer editor de texto.

## Configuração do Menu de Contexto

Para usar o software diretamente pelo menu de contexto, siga estes passos:

1. Abra o Editor de Registro (regedit).
2. Navegue até `HKEY_CLASSES_ROOT\Directory\shell\`.
3. Crie uma nova chave com o nome desejado para o menu.
4. Dentro dessa chave, adicione o comando que aponta para o executável do software.

### Exemplo:

```registry
[HKEY_CLASSES_ROOT\Directory\shell\MultiFolderSelector]
@="Copiar Estrutura de Pastas"
[HKEY_CLASSES_ROOT\Directory\shell\MultiFolderSelector\command]
@="\"C:\\Caminho\\para\\seu\\programa.exe\" \"%1\""
```

## Contribuição

Para contribuir, faça um fork do repositório e envie um pull request com suas modificações. Todos os tipos de contribuição são bem-vindos!

## Licença

Este projeto está licenciado sob a Licença MIT.
