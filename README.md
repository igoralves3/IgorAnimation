# IgorAnimation

Bem-vindo ao repositório **IgorAnimation**! Este projeto é uma biblioteca de animações desenvolvida como trabalho de faculdade para a disciplina de Animação Computadorizada. O projeto foi realizado pelos alunos:

- Cindi Saicosque
- Igor Alves
- Guilherme

## Índice

- [Características](#características)
- [Instalação](#instalação)
- [Uso](#uso)
- [Contribuição](#contribuição)
- [Licença](#licença)

## Características

- Animações personalizáveis
- Fácil de integrar em projetos existentes
- Suporte a múltiplos tipos de animação (ex: transições, efeitos de hover)
- Documentação clara e exemplos práticos

## Instalação

Para instalar o IgorAnimation, você pode usar o npm ou baixar os arquivos diretamente.

### Usando npm

```bash
npm install igoranime
```

### Ou baixar manualmente

1. Acesse a [página de releases](https://github.com/igoralves3/IgorAnimation/releases).
2. Baixe a versão mais recente e extraia os arquivos.

## Uso

Após a instalação, você pode começar a usar a biblioteca em seu projeto. Aqui está um exemplo básico de como utilizar:

```javascript
import { animate } from 'igoranime';

// Exemplo de animação
animate('.element', {
  opacity: 1,
  transform: 'translateY(0)',
}, {
  duration: 1000,
  easing: 'ease-in-out'
});
```

Para mais exemplos e detalhes, consulte a [documentação completa](link para a documentação).

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests. Para contribuir, siga estas etapas:

1. Fork o projeto
2. Crie sua branch (`git checkout -b feature/nova-funcionalidade`)
3. Commit suas alterações (`git commit -m 'Adicionando nova funcionalidade'`)
4. Push para a branch (`git push origin feature/nova-funcionalidade`)
5. Abra um Pull Request

## Assets externos
https://assetstore.unity.com/packages/tools/modeling/deform-148425

## Licença

Este projeto está licenciado sob a [Licença MIT](LICENSE).
