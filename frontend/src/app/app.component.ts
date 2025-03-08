import {Component, inject, OnInit, signal} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {HttpClient} from "@angular/common/http";

@Component({
    selector: 'app-root',
    imports: [RouterOutlet],
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
    title = signal("Welcome to my website!");

    recipesFound = signal(-1);

    httpClient = inject(HttpClient);

    ngOnInit() {
        this.httpClient
            .get<RecipesResponse>('/api/recipes-module/recipes')
            .subscribe((recipes) => {
                this.recipesFound.set(recipes.recipes.length);
            });
    }
}

export interface RecipesResponse {
    recipes: Recipe[];
    nextKey: number;
}

export interface Recipe {
    id: number;
}
