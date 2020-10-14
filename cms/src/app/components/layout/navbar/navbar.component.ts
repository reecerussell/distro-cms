import { Component, OnInit, Input } from "@angular/core";
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import AppState from 'src/app/store/app.state';
import * as DictionaryActions from 'src/app/store/dictionary/dictionary.action';
import { DictionaryState } from 'src/app/store/dictionary/dictionary.state';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  dictionaryState$: Observable<DictionaryState>
  culture: string;

  @Input() toggleMenu;
  
  constructor(private store: Store<AppState>, private router: Router) {
      this.dictionaryState$ = store.select(state => state.dictionary)
  }

  ngOnInit(): void {
      this.store.dispatch(new DictionaryActions.GetDropdownItems())

      this.culture = localStorage.getItem("DICTIONARY_CULTURE") ?? navigator.language;
      this.dictionaryState$.subscribe(state => {
          this.culture = state?.culture ?? localStorage.getItem("DICTIONARY_CULTURE") ?? navigator.language;
          console.log("Sub: " + this.culture);
      })
  }

  onCultureChange(e): void {
      this.store.dispatch(new DictionaryActions.SetCulture(e.target.value));
  }
}
