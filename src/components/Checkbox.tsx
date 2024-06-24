import { Component } from 'react';

interface CheckboxProps {
  id: string;
  label: string;
  checked: boolean;
  className?: string;
  onChange: () => void;
}

export default class Checkbox extends Component<CheckboxProps> {
  render() {
    const { id, label, checked, className, onChange } = this.props;

    return (
      <div className={className ?? 'flex items-center gap-2'}>
        <input id={id} type='checkbox' className='checkbox' checked={checked} onChange={() => onChange()} />
        <label htmlFor={id}>{label}</label>
      </div>
    );
  }
}
